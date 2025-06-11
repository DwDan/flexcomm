using Confluent.Kafka;
using FC.BuildingBlocks.Domain.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FC.BuildingBlocks.Infrastructure.Messaging.Kafka
{
    public class KafkaProducerWrapper : IProducerWrapper
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerWrapper> _logger;

        public KafkaProducerWrapper(
            IOptions<KafkaProducerSettings> options,
            ILogger<KafkaProducerWrapper> logger)
        {
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                ClientId = options.Value.ClientId,
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken);

                _logger.LogInformation("Mensagem publicada com sucesso no tópico {Topic}", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem no tópico {Topic}", topic);
                throw;
            }
        }

        public void Dispose() => _producer.Dispose();
    }
}