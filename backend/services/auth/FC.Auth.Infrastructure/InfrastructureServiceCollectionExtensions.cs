using FC.Auth.Domain.Repositories;
using FC.Auth.Infrastructure.Repositories;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Security;
using FC.BuildingBlocks.Infrastructure.Configuration;
using FC.BuildingBlocks.Infrastructure.Messaging;
using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using FC.BuildingBlocks.Infrastructure.Messaging.Kafka;
using FC.BuildingBlocks.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Auth.Infrastructure
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AutenticacaoContext>(options =>
                options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly(typeof(InfrastructureServiceCollectionExtensions).Assembly.FullName)));

            services.EnsureSettings<KafkaProducerSettings, KafkaProducerSettingsValidator>(configuration, "EventBus");

            services.AddJwtAuthentication(configuration);
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<AutenticacaoContext>());
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPasswordHash, BCryptPasswordHash>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEmailConfirmationTokenValidator, EmailConfirmationTokenValidator>();

            services.AddScoped<IEventBusProducer, EventBusProducer>();
            services.AddScoped<IProducerWrapper, KafkaProducerWrapper>();
            services.AddScoped<ICorrelationContext, CorrelationContext>();

            return services;
        }
    }
}