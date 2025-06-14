namespace FC.BuildingBlocks.Domain.Messaging
{
    public interface ICorrelationContext
    {
        string CorrelationId { get; }
        void Set(string correlationId);
    }
}
