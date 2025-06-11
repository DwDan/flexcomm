using Confluent.Kafka;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FC.BuildingBlocks.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerWrapper : IConsumerWrapper
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<EventBusConsumer> _logger;
        private readonly IOptions<KafkaConsumerSettings> _options;

        public KafkaConsumerWrapper(
            IOptions<KafkaConsumerSettings> options, 
            ILogger<EventBusConsumer> logger)
        {
            _logger = logger;
            _options = options;

            var settings = options.Value;

            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void Subscribe() => _consumer.Subscribe(_options.Value.Topic);

        public string? Consume(CancellationToken cancellationToken)
        {
            try
            {
                var result = _consumer.Consume(cancellationToken);
                return result?.Message?.Value;
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Erro ao consumir mensagem do Kafka.");
                throw;
            }
        }

        public void Close() => _consumer.Close();

        public void Dispose() => _consumer.Dispose();
    }
}