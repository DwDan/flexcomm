namespace FC.BuildingBlocks.Domain.Messaging.Events
{
    public class EnviarEmailEvent : IntegrationEvent
    {
        public required string To { get; init; }
        public required string Subject { get; init; }
        public required string HtmlBody { get; init; }
    }
}
