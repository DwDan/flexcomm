using FC.BuildingBlocks.Domain.Messaging.Events;

namespace FC.BuildingBlocks.Domain.Messaging
{
    public class IntegrationEventEnvelope<T> where T : IntegrationEvent
    {
        public string CorrelationId { get; set; }
        public string EventType => typeof(T).Name;
        public T Data { get; init; }

        public IntegrationEventEnvelope(T data, string correlationId)
        {
            Data = data;
            CorrelationId = correlationId;
        }
    }
}
