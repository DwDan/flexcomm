using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace FC.BuildingBlocks.Integration.Tests
{
    public class IntegrationTestsFixture<TProgram, TDbContext> : IDisposable
        where TProgram : class
        where TDbContext : DbContext
    {
        public HttpClient Client { get; }
        public WebApplicationFactory<TProgram> Factory { get; }

        private readonly PostgreSqlContainer _dbContainer;

        public IntegrationTestsFixture(string migrationsAssembly)
        {
            _dbContainer = StartContainer();
            Factory = ConfigureFactory(migrationsAssembly);
            Client = CreateClient();
            InitializeDatabase();
        }

        private PostgreSqlContainer StartContainer()
        {
            var container = new PostgreSqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithImage("postgres:13")
                .Build();

            container.StartAsync().GetAwaiter().GetResult();
            return container;
        }

        private WebApplicationFactory<TProgram> ConfigureFactory(string migrationsAssembly)
        {
            return new WebApplicationFactory<TProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d =>
                            d.ServiceType == typeof(DbContextOptions<TDbContext>));

                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<TDbContext>(options =>
                            options.UseNpgsql(_dbContainer.GetConnectionString(),
                                b => b.MigrationsAssembly(migrationsAssembly)));

                        services.AddScoped<DbContext>(provider =>
                            provider.GetRequiredService<TDbContext>());
                    });
                });
        }

        private HttpClient CreateClient()
        {
            return Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost"),
                AllowAutoRedirect = true,
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            });
        }

        private void InitializeDatabase()
        {
            using var scope = Factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DbContext>();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
            _dbContainer.StopAsync().GetAwaiter().GetResult();
        }
    }
}
