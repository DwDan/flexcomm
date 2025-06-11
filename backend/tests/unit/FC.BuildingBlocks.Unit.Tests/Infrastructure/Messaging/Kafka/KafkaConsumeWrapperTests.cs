using Confluent.Kafka;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using FC.BuildingBlocks.Infrastructure.Messaging.Kafka;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.Kafka.Tests
{
    public class KafkaConsumerWrapperTests
    {
        [Fact(DisplayName = "Deve assinar o tópico corretamente")]
        public void Subscribe_DeveAssinarOTopico()
        {
            var settings = new KafkaConsumerSettings
            {
                Topic = "authorization"
            };

            var consumer = Substitute.For<IConsumer<Ignore, string>>();
            var wrapper = new KafkaConsumerWrapperFake(settings, consumer);

            wrapper.Subscribe();

            consumer.Received(1).Subscribe("authorization");
        }

        [Fact(DisplayName = "Deve retornar valor da mensagem consumida")]
        public void Consume_DeveRetornarMensagem()
        {
            var msg = new Message<Ignore, string> { Value = "mensagem-teste" };
            var consumeResult = new ConsumeResult<Ignore, string> { Message = msg };

            var consumer = Substitute.For<IConsumer<Ignore, string>>();
            consumer.Consume(Arg.Any<CancellationToken>()).Returns(consumeResult);

            var wrapper = new KafkaConsumerWrapperFake(new KafkaConsumerSettings(), consumer);

            var result = wrapper.Consume(CancellationToken.None);

            Assert.Equal("mensagem-teste", result);
        }

        [Fact(DisplayName = "Deve lançar exceção e logar erro ao consumir")]
        public void Consume_ComExcecao_DeveLogarEReLancar()
        {
            var exception = new ConsumeException(new ConsumeResult<byte[], byte[]>(), new Error(ErrorCode.Unknown, "Erro"));
            var consumer = Substitute.For<IConsumer<Ignore, string>>();
            consumer.Consume(Arg.Any<CancellationToken>()).Throws(exception);

            var logger = Substitute.For<ILogger<EventBusConsumer>>();
            var wrapper = new KafkaConsumerWrapperFake(new KafkaConsumerSettings(), consumer, logger);

            var ex = Assert.Throws<ConsumeException>(() => wrapper.Consume(CancellationToken.None));
            Assert.Equal(exception, ex);

            logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Erro ao consumir mensagem")),
                exception,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve chamar Close e Dispose no consumer")]
        public void Close_Dispose_DeveChamarNoKafka()
        {
            var consumer = Substitute.For<IConsumer<Ignore, string>>();
            var wrapper = new KafkaConsumerWrapperFake(new KafkaConsumerSettings(), consumer);

            wrapper.Close();
            wrapper.Dispose();

            consumer.Received(1).Close();
            consumer.Received(1).Dispose();
        }

        private class KafkaConsumerWrapperFake : IConsumerWrapper
        {
            private readonly KafkaConsumerSettings _settings;
            private readonly IConsumer<Ignore, string> _consumer;
            private readonly ILogger<EventBusConsumer>? _logger;

            public KafkaConsumerWrapperFake(KafkaConsumerSettings settings, IConsumer<Ignore, string> consumer, ILogger<EventBusConsumer>? logger = null)
            {
                _settings = settings;
                _consumer = consumer;
                _logger = logger;
            }

            public void Subscribe() => _consumer.Subscribe(_settings.Topic);

            public string? Consume(CancellationToken cancellationToken)
            {
                try
                {
                    var result = _consumer.Consume(cancellationToken);
                    return result?.Message?.Value;
                }
                catch (ConsumeException ex)
                {
                    _logger?.LogError(ex, "Erro ao consumir mensagem do Kafka.");
                    throw;
                }
            }

            public void Close() => _consumer.Close();
            public void Dispose() => _consumer.Dispose();
        }
    }
}
