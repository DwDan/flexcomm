using FC.BuildingBlocks.Domain.Messaging.Events;

namespace FC.BuildingBlocks.Domain.Messaging
{
    public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        Task HandleAsync(TEvent evento, CancellationToken cancellationToken);
    }
}
