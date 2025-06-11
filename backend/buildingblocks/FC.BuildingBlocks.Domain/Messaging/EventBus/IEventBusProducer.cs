using FC.BuildingBlocks.Domain.Messaging.Events;

namespace FC.BuildingBlocks.Domain.Messaging.EventBus
{
    public interface IEventBusProducer
    {
        Task PublicarAsync<T>(string topic, T message) where T : IntegrationEvent;
    }
}
