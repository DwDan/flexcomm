using System.Text.Json;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FC.BuildingBlocks.Infrastructure.Messaging
{
    public class EventBusDispatcher : IEventBusDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBusDispatcher> _logger;

        public EventBusDispatcher(
            IServiceProvider serviceProvider,
            ILogger<EventBusDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task DispatchAsync(string rawMessage, CancellationToken cancellationToken)
        {
            var doc = JsonDocument.Parse(rawMessage);

            var typeName = doc.RootElement.GetProperty("EventType").GetString();
            var data = doc.RootElement.GetProperty("Data").GetRawText();

            if (string.IsNullOrWhiteSpace(typeName))
            {
                _logger.LogWarning("Evento sem tipo especificado.");
                return;
            }

            var eventType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == typeName);

            if (eventType is null)
            {
                _logger.LogWarning("Tipo de evento desconhecido: {TypeName}", typeName);
                return;
            }

            var evento = (IntegrationEvent?)JsonSerializer.Deserialize(data, eventType);
            if (evento is null)
            {
                _logger.LogWarning("Falha ao desserializar evento do tipo {TypeName}", typeName);
                return;
            }

            _logger.LogInformation("Iniciando o dispatch do evento {EventType}", typeName);

            var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService(handlerType);

            if (handler is null)
            {
                _logger.LogWarning("Handler não encontrado para tipo {EventType}", typeName);
                return;
            }

            try
            {
                dynamic dynamicHandler = handler;
                await dynamicHandler.HandleAsync((dynamic)evento, cancellationToken);

                _logger.LogInformation("Handler executado com sucesso para {EventType}", typeName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar evento {EventType}", typeName);
                throw;
            }
        }
    }
}
