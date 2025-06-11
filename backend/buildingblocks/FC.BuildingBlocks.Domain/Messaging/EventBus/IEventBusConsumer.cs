namespace FC.BuildingBlocks.Domain.Messaging.EventBus
{
    public interface IEventBusConsumer
    {
        Task StartAsync(CancellationToken cancellationToken, bool runOnce = false);
    }
}
