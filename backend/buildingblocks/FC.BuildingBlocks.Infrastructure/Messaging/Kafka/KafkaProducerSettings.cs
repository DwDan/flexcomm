namespace FC.BuildingBlocks.Infrastructure.Messaging.Kafka
{
    public class KafkaProducerSettings
    {
        public string BootstrapServers { get; set; } = default!;
        public string ClientId { get; set; } = default!;
    }
}
