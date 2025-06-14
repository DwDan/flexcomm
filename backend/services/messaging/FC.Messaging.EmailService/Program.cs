using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.BuildingBlocks.Infrastructure.Configuration;
using FC.BuildingBlocks.Infrastructure.Messaging;
using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using FC.BuildingBlocks.Infrastructure.Messaging.Kafka;
using FC.Messaging.EmailService.Application;
using FC.Messaging.EmailService.Domain;
using FC.Messaging.EmailService.Infrastructure;
using FC.Messaging.EmailService.Infrastructure.Smtp;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace FC.Messaging.EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            // Logging
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration));

            // Serviços
            builder.Services.EnsureSettings<SmtpSettings, SmtpSettingsValidator>(builder.Configuration, "Smtp");
            builder.Services.EnsureSettings<EventBusConsumerSettings, EventBusConsumerSettingsValidator>(builder.Configuration, "EventBus");
            builder.Services.EnsureSettings<KafkaConsumerSettings, KafkaConsumerSettingsValidator>(builder.Configuration, "EventBus");

            builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
            builder.Services.AddScoped<IEventBusDispatcher, EventBusDispatcher>();
            builder.Services.AddScoped<IEventBusConsumer, EventBusConsumer>();
            builder.Services.AddScoped<IConsumerWrapper, KafkaConsumerWrapper>();
            builder.Services.AddScoped<IIntegrationEventHandler<EnviarEmailEvent>, EnviarEmailHandler>();
            builder.Services.AddScoped<ICorrelationContext, CorrelationContext>();
            builder.Services.AddHostedService<MessageConsumerService>();

            var app = builder.Build();

            app.Run();
        }
    }
}