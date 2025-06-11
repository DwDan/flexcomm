using System.Text.Json;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.EventBus.Tests
{
    public class EventBusConsumerTests
    {
        private static EventBusConsumer CreateConsumer(
            out IConsumerWrapper consumerWrapper,
            out IEventBusDispatcher dispatcher,
            out ICorrelationContext correlationContext,
            out ILogger<EventBusConsumer> logger,
            string? message)
        {
            dispatcher = Substitute.For<IEventBusDispatcher>();
            correlationContext = Substitute.For<ICorrelationContext>();
            logger = Substitute.For<ILogger<EventBusConsumer>>();
            consumerWrapper = Substitute.For<IConsumerWrapper>();

            var options = Substitute.For<IOptions<EventBusConsumerSettings>>();
            options.Value.Returns(new EventBusConsumerSettings
            {
                Topic = "test-topic"
            });

            consumerWrapper.Consume(Arg.Any<CancellationToken>()).Returns(_ => message);

            return new EventBusConsumer(
                dispatcher, options, logger, correlationContext, consumerWrapper);
        }

        [Fact]
        public async Task MensagemNula_DeveLogarAviso()
        {
            var consumer = CreateConsumer(out var wrapper, out _, out _, out var logger, null);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(50);

            await consumer.StartAsync(cts.Token);

            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(x => x.ToString()!.Contains("Mensagem consumida é nula")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task MensagemComCorrelationId_DeveSetarContexto()
        {
            var correlationId = Guid.NewGuid().ToString();
            var raw = JsonSerializer.Serialize(new
            {
                EventType = "Teste",
                Data = "{}",
                CorrelationId = correlationId
            });

            var consumer = CreateConsumer(out _, out _, out var context, out _, raw);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(50);

            await consumer.StartAsync(cts.Token);

            context.Received().Set(correlationId);
        }

        [Fact]
        public async Task MensagemSemCorrelationId_DeveGerarNovoGuid()
        {
            var raw = JsonSerializer.Serialize(new
            {
                EventType = "Teste",
                Data = "{}"
            });

            var consumer = CreateConsumer(out _, out _, out var context, out _, raw);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(50);

            await consumer.StartAsync(cts.Token);

            Guid result;

            context.Received().Set(Arg.Is<string>(id => Guid.TryParse(id, out result)));
        }

        [Fact]
        public async Task MensagemJsonInvalida_DeveLogarErro()
        {
            var raw = "json-malformado{";

            var consumer = CreateConsumer(out _, out _, out _, out var logger, raw);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(50);

            await consumer.StartAsync(cts.Token);

            logger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<JsonException>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task MensagemValida_DeveExecutarDispatcher()
        {
            var raw = JsonSerializer.Serialize(new
            {
                EventType = "Teste",
                Data = "{}"
            });

            var consumer = CreateConsumer(out _, out var dispatcher, out _, out _, raw);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(50);

            await consumer.StartAsync(cts.Token);

            await dispatcher.Received().DispatchAsync(raw, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CancelamentoSolicitado_DeveEncerrarComGrace()
        {
            // Arrange
            var consumer = CreateConsumer(out var wrapper, out _, out _, out var logger, message: null);

            wrapper
                .Consume(Arg.Any<CancellationToken>())
                .Returns(_ => throw new OperationCanceledException());

            var cts = new CancellationTokenSource(); 

            // Act
            await consumer.StartAsync(cts.Token);

            // Assert
            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(m => m.ToString()!.Contains("Cancelamento solicitado")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task DispatcherLancaErro_DeveLogarErro()
        {
            var raw = JsonSerializer.Serialize(new
            {
                EventType = "Teste",
                Data = "{}"
            });

            var consumer = CreateConsumer(out _, out var dispatcher, out _, out var logger, raw);

            dispatcher
                .DispatchAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns<Task>(_ => throw new InvalidOperationException("erro"));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(50);

            await consumer.StartAsync(cts.Token);

            logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Erro inesperado")),
                Arg.Any<InvalidOperationException>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}
