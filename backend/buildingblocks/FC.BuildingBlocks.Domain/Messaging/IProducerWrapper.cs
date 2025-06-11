namespace FC.BuildingBlocks.Domain.Messaging
{
    public interface IProducerWrapper : IDisposable
    {
        Task PublishAsync(string topic, string message, CancellationToken cancellationToken = default);
    }
}