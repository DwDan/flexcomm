namespace FC.BuildingBlocks.Domain.Messaging.Events
{
    public abstract class IntegrationEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
        public string? EventType => GetType().Name;
    }
}
