using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;

namespace FC.BuildingBlocks.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerSettings : EventBusConsumerSettings
    {
        public string BootstrapServers { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string GroupId { get; set; } = null!;
    }
}
