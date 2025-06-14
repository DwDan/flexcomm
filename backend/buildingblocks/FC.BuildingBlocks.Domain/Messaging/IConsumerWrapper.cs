namespace FC.BuildingBlocks.Domain.Messaging
{
    public interface IConsumerWrapper : IDisposable
    {
        void Subscribe();
        string? Consume(CancellationToken cancellationToken);
        void Close();
    }
}