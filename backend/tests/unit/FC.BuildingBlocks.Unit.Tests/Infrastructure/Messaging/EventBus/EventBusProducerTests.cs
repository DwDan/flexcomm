using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.EventBus.Tests
{
    public class EventBusProducerTests
    {
        private class UsuarioCriadoEvent : IntegrationEvent
        {
            public string Nome { get; init; }

            public UsuarioCriadoEvent(string nome)
            {
                Nome = nome;
            }
        }

        [Fact(DisplayName = "Deve serializar e publicar evento com sucesso")]
        public async Task PublicarAsync_EventoValido_DeveChamarPublishAsync()
        {
            // Arrange
            var producerWrapper = Substitute.For<IProducerWrapper>();
            var correlationContext = Substitute.For<ICorrelationContext>();
            var logger = Substitute.For<ILogger<EventBusProducer>>();

            correlationContext.CorrelationId.Returns("correlation-123");

            var producer = new EventBusProducer(producerWrapper, correlationContext, logger);

            var evento = new UsuarioCriadoEvent("Daniel");
            var topic = "authorization";

            // Act
            await producer.PublicarAsync(topic, evento);

            // Assert
            await producerWrapper.Received(1).PublishAsync(
                Arg.Is<string>(t => t == topic),
                Arg.Is<string>(json =>
                    json.Contains("UsuarioCriadoEvent") &&
                    json.Contains("Daniel") &&
                    json.Contains("correlation-123")),
                Arg.Any<CancellationToken>());

            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Iniciando publicação")),
                null,
                Arg.Any<Func<object, Exception?, string>>());

            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("publicado com sucesso")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}