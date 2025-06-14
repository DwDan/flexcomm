using Confluent.Kafka;
using Confluent.Kafka.Admin;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace FC.BuildingBlocks.Integration.Tests.Fixture;

public class IntegrationEndToEndFixture : IDisposable
{
    public Dictionary<string, object> Services { get; } = new();
    public Dictionary<string, HttpClient> Clients { get; } = new();
    public PostgreSqlContainer? DbContainer { get; private set; }
    public KafkaContainer? KafkaContainer { get; private set; }

    public void WithEventBus() => EnsureKafkaStarted();

    public void WithTopic(string topic)
    {
        EnsureKafkaStarted();
        CriarTopicoAsync(KafkaContainer!.GetBootstrapAddress(), topic).GetAwaiter().GetResult();
    }

    public void WithProducer<TProgram, TDbContext>(string name, string migrationsAssembly, Action<IServiceCollection>? configure = null)
        where TProgram : class
        where TDbContext : DbContext =>
        SetupService<TProgram, TDbContext>(name, migrationsAssembly, configure, isConsumer: false, runExactly: 1);

    public void WithConsumer<TProgram, TDbContext>(string name, string migrationsAssembly, Action<IServiceCollection>? configure = null, int runExactly = 1)
        where TProgram : class
        where TDbContext : DbContext =>
        SetupService<TProgram, TDbContext>(name, migrationsAssembly, configure, isConsumer: true, startConsumer: true, runExactly);

    public void WithWorker<TProgram>(string name, bool isProducer, bool isConsumer, Action<IServiceCollection>? configure = null, int runExactly = 1)
        where TProgram : class
    {
        var factory = CreateFactory<TProgram>(services =>
        {
            configure?.Invoke(services);
            if (isConsumer) RemoverBackgroundServices(services);
        });

        Services[name] = factory;
        Clients[name] = factory.CreateClient();

        if (isConsumer) 
            StartConsumer(factory);
    }

    private void SetupService<TProgram, TDbContext>(string name, string migrationsAssembly, Action<IServiceCollection>? configure, bool isConsumer, bool startConsumer = false, int runExactly = 1)
        where TProgram : class
        where TDbContext : DbContext
    {
        EnsureDbStarted();

        var factory = CreateFactory<TProgram>(services =>
        {
            SubstituirDbContext<TDbContext>(services, migrationsAssembly);
            configure?.Invoke(services);
            if (isConsumer) RemoverBackgroundServices(services);
        });

        Services[name] = factory;
        Clients[name] = factory.CreateClient();

        InitializeDatabase<TProgram, TDbContext>(name);

        if (startConsumer)
            StartConsumer(factory, runExactly);
    }

    private WebApplicationFactory<TProgram> CreateFactory<TProgram>(Action<IServiceCollection> configureServices)
        where TProgram : class
    {
        return new WebApplicationFactory<TProgram>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration((_, config) =>
            {
                var settings = new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = DbContainer?.GetConnectionString() ?? string.Empty,
                    ["EventBus:BootstrapServers"] = KafkaContainer?.GetBootstrapAddress() ?? string.Empty
                };

                config.AddInMemoryCollection(settings!);
            });

            builder.ConfigureServices(configureServices);
        });
    }

    private void SubstituirDbContext<TDbContext>(IServiceCollection services, string assembly)
        where TDbContext : DbContext
    {
        var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<TDbContext>));
        if (descriptor != null) services.Remove(descriptor);

        services.AddDbContext<TDbContext>(opts =>
            opts.UseNpgsql(DbContainer!.GetConnectionString(),
                builder => builder.MigrationsAssembly(assembly)));
    }

    private void RemoverBackgroundServices(IServiceCollection services)
    {
        var handlers = services
            .Where(s => typeof(IEventBackgroundService).IsAssignableFrom(s.ImplementationType ?? typeof(object)))
            .ToList();

        foreach (var h in handlers)
            services.Remove(h);
    }

    private void EnsureDbStarted()
    {
        if (DbContainer != null) return;

        DbContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithImage("postgres:13")
            .Build();

        DbContainer.StartAsync().GetAwaiter().GetResult();
    }

    private void EnsureKafkaStarted()
    {
        if (KafkaContainer != null) return;

        KafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.2.1")
            .WithCleanUp(true)
            .Build();

        KafkaContainer.StartAsync().GetAwaiter().GetResult();
    }

    private async Task CriarTopicoAsync(string bootstrap, string topic)
    {
        var config = new AdminClientConfig { BootstrapServers = bootstrap };
        using var adminClient = new AdminClientBuilder(config).Build();

        try
        {
            await adminClient.CreateTopicsAsync(new[]
            {
                new TopicSpecification { Name = topic, NumPartitions = 1, ReplicationFactor = 1 }
            });
        }
        catch (CreateTopicsException ex) when (ex.Results[0].Error.Code == ErrorCode.TopicAlreadyExists) { }
    }

    private void StartConsumer<TProgram>(WebApplicationFactory<TProgram> factory, int runExactly = 1) where TProgram : class
    {
        Task.Run(async () =>
        {
            using var scope = factory.Services.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IEventBusConsumer>();
            await consumer.StartAsync(CancellationToken.None, runExactly: runExactly);
        });
    }

    public void InitializeDatabase<TProgram, TDbContext>(string name)
        where TProgram : class
        where TDbContext : DbContext
    {
        using var scope = GetFactory<TProgram>(name).Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public WebApplicationFactory<TProgram> GetFactory<TProgram>(string name) where TProgram : class
        => (WebApplicationFactory<TProgram>)Services[name];

    public void Dispose()
    {
        foreach (var client in Clients.Values)
            client.Dispose();

        foreach (var disposable in Services.Values.OfType<IDisposable>())
            disposable.Dispose();

        DbContainer?.StopAsync().GetAwaiter().GetResult();
        KafkaContainer?.StopAsync().GetAwaiter().GetResult();
    }
}
