using System.Text.Json;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using Microsoft.Extensions.Logging;

namespace FC.BuildingBlocks.Infrastructure.Messaging.EventBus
{
    public class EventBusProducer : IEventBusProducer
    {
        private readonly IProducerWrapper _producerWrapper;
        private readonly ICorrelationContext _correlationContext;
        private readonly ILogger<EventBusProducer> _logger;

        public EventBusProducer(
            IProducerWrapper producerWrapper,
            ICorrelationContext correlationContext,
            ILogger<EventBusProducer> logger)
        {
            _producerWrapper = producerWrapper;
            _correlationContext = correlationContext;
            _logger = logger;
        }

        public async Task PublicarAsync<T>(string topic, T message) where T : IntegrationEvent
        {
            var envelope = new IntegrationEventEnvelope<T>(message, _correlationContext.CorrelationId);
            var payload = JsonSerializer.Serialize(envelope);

            _logger.LogInformation("Iniciando publicação do evento {EventType} no tópico {Topic}",
                typeof(T).Name, topic);

            await _producerWrapper.PublishAsync(topic, payload);

            _logger.LogInformation("Evento {EventType} publicado com sucesso no tópico {Topic}",
                typeof(T).Name, topic);
        }
    }
}