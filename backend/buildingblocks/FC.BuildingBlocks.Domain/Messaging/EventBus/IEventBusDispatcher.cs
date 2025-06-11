namespace FC.BuildingBlocks.Domain.Messaging.EventBus
{
    public interface IEventBusDispatcher
    {
        Task DispatchAsync(string rawMessage, CancellationToken cancellationToken);
    }
}
