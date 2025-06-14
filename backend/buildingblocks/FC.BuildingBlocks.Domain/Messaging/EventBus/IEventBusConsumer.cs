namespace FC.BuildingBlocks.Domain.Messaging.EventBus
{
    public interface IEventBusConsumer
    {
        Task StartAsync(CancellationToken cancellationToken, int runExactly = 0);
    }
}
