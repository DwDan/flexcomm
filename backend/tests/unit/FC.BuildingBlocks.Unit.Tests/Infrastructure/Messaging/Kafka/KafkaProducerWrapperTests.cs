using Confluent.Kafka;
using FC.BuildingBlocks.Infrastructure.Messaging.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.Kafka.Tests
{
    public class KafkaProducerWrapperTests
    {
        [Fact(DisplayName = "Deve publicar mensagem com sucesso e logar")]
        public async Task PublishAsync_MensagemValida_DeveChamarProducer()
        {
            // Arrange
            var producer = Substitute.For<IProducer<Null, string>>();
            var logger = Substitute.For<ILogger<KafkaProducerWrapper>>();

            var wrapper = new KafkaProducerWrapperFake(producer, logger);

            // Act
            await wrapper.PublishAsync("authorization", "mensagem");

            // Assert
            await producer.Received(1).ProduceAsync(
                "authorization",
                Arg.Is<Message<Null, string>>(m => m.Value == "mensagem"),
                Arg.Any<CancellationToken>());

            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Mensagem publicada com sucesso")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve logar e relançar exceção se falhar ao publicar")]
        public async Task PublishAsync_Erro_DeveLogarErroELancar()
        {
            // Arrange
            var producer = Substitute.For<IProducer<Null, string>>();
            var logger = Substitute.For<ILogger<KafkaProducerWrapper>>();
            var exception = new InvalidOperationException("falha");

            producer.ProduceAsync(Arg.Any<string>(), Arg.Any<Message<Null, string>>(), Arg.Any<CancellationToken>())
                    .Returns<Task>(_ => throw exception);

            var wrapper = new KafkaProducerWrapperFake(producer, logger);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                wrapper.PublishAsync("authorization", "mensagem"));

            Assert.Equal(exception, ex);

            logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Erro ao publicar mensagem")),
                exception,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve chamar Dispose no producer")]
        public void Dispose_DeveChamarDisposeInterno()
        {
            var producer = Substitute.For<IProducer<Null, string>>();
            var logger = Substitute.For<ILogger<KafkaProducerWrapper>>();
            var wrapper = new KafkaProducerWrapperFake(producer, logger);

            wrapper.Dispose();

            producer.Received(1).Dispose();
        }

        private class KafkaProducerWrapperFake : KafkaProducerWrapper
        {
            private readonly IProducer<Null, string> _injected;

            public KafkaProducerWrapperFake(IProducer<Null, string> producer, ILogger<KafkaProducerWrapper> logger)
                : base(CreateFakeOptions(), logger)
            {
                _injected = producer;
                OverrideProducer(producer);
            }

            private static IOptions<KafkaProducerSettings> CreateFakeOptions()
            {
                return Options.Create(new KafkaProducerSettings
                {
                    BootstrapServers = "localhost:9092",
                    ClientId = "test"
                });
            }

            public void OverrideProducer(IProducer<Null, string> producer)
            {
                typeof(KafkaProducerWrapper)
                    .GetField("_producer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .SetValue(this, producer);
            }
        }
    }
}
