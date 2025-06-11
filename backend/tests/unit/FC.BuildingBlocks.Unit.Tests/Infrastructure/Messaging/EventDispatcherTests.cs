using System.Text.Json;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.BuildingBlocks.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.Tests
{
    public class EventDispatcherTests
    {
        public class SampleEvent : IntegrationEvent
        {
            public string Nome { get; init; }

            public SampleEvent(string nome)
            {
                Nome = nome;
                Id = Guid.NewGuid();
                OccurredAt = DateTime.UtcNow;
            }
        }

        public class SampleEventHandler : IIntegrationEventHandler<SampleEvent>
        {
            public Task HandleAsync(SampleEvent @event, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        public static string BuildRawMessage(string typeName, object data)
        {
            return JsonSerializer.Serialize(new
            {
                EventType = typeName,
                Data = data   
            });
        }

        [Fact(DisplayName = "Deve logar e retornar se EventType estiver vazio")]
        public async Task DispatchAsync_EventTypeVazio_DeveLogarAviso()
        {
            var logger = Substitute.For<ILogger<EventBusDispatcher>>();
            var sp = Substitute.For<IServiceProvider>();
            var dispatcher = new EventBusDispatcher(sp, logger);

            var rawMessage = JsonSerializer.Serialize(new { EventType = "", Data = "{}" });

            await dispatcher.DispatchAsync(rawMessage, CancellationToken.None);

            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("sem tipo especificado")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve logar se tipo não for encontrado")]
        public async Task DispatchAsync_TipoDesconhecido_DeveLogarAviso()
        {
            var logger = Substitute.For<ILogger<EventBusDispatcher>>();
            var sp = Substitute.For<IServiceProvider>();
            var dispatcher = new EventBusDispatcher(sp, logger);

            var rawMessage = BuildRawMessage("TipoInexistente", new { });

            await dispatcher.DispatchAsync(rawMessage, CancellationToken.None);

            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Tipo de evento desconhecido")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve logar se falhar na desserialização")]
        public async Task DispatchAsync_DesserializacaoFalha_DeveLogarAviso()
        {
            var logger = Substitute.For<ILogger<EventBusDispatcher>>();
            var sp = Substitute.For<IServiceProvider>();
            var dispatcher = new EventBusDispatcher(sp, logger);

            var rawMessage = JsonSerializer.Serialize(new
            {
                EventType = nameof(SampleEvent),
                Data = "INVALID_JSON"
            });

            var ex = await Assert.ThrowsAsync<JsonException>(() =>
                dispatcher.DispatchAsync(rawMessage, CancellationToken.None));

            Assert.NotNull(ex);
        }

        [Fact(DisplayName = "Deve logar se handler não for encontrado")]
        public async Task DispatchAsync_HandlerNaoRegistrado_DeveLogarAviso()
        {
            var logger = Substitute.For<ILogger<EventBusDispatcher>>();

            var scope = Substitute.For<IServiceScope>();
            scope.ServiceProvider.GetService(Arg.Any<Type>()).Returns(null);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(scope);

            var sp = Substitute.For<IServiceProvider>();
            sp.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);

            var dispatcher = new EventBusDispatcher(sp, logger);

            var rawMessage = BuildRawMessage(nameof(SampleEvent), new SampleEvent("Daniel"));

            await dispatcher.DispatchAsync(rawMessage, CancellationToken.None);

            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handler não encontrado")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve despachar evento com sucesso")]
        public async Task DispatchAsync_EventoValido_DeveExecutarHandler()
        {
            var logger = Substitute.For<ILogger<EventBusDispatcher>>();

            var handler = Substitute.For<IIntegrationEventHandler<SampleEvent>>();

            var scope = Substitute.For<IServiceScope>();
            scope.ServiceProvider.GetService(Arg.Any<Type>()).Returns(handler);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(scope);

            var sp = Substitute.For<IServiceProvider>();
            sp.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);

            var dispatcher = new EventBusDispatcher(sp, logger);

            var evento = new SampleEvent("Daniel");
            var rawMessage = BuildRawMessage(nameof(SampleEvent), evento);

            await dispatcher.DispatchAsync(rawMessage, CancellationToken.None);

            await handler.Received(1).HandleAsync(Arg.Any<SampleEvent>(), Arg.Any<CancellationToken>());
            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handler executado com sucesso")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact(DisplayName = "Deve lançar exceção se handler lançar erro")]
        public async Task DispatchAsync_HandlerFalha_DeveLancarExcecaoELogarErro()
        {
            var logger = Substitute.For<ILogger<EventBusDispatcher>>();

            var handler = Substitute.For<IIntegrationEventHandler<SampleEvent>>();
            handler.HandleAsync(Arg.Any<SampleEvent>(), Arg.Any<CancellationToken>())
                   .Returns<Task>(x => throw new InvalidOperationException("Erro de teste"));

            var scope = Substitute.For<IServiceScope>();
            scope.ServiceProvider.GetService(Arg.Any<Type>()).Returns(handler);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(scope);

            var sp = Substitute.For<IServiceProvider>();
            sp.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);

            var dispatcher = new EventBusDispatcher(sp, logger);
            var evento = new SampleEvent("Daniel");
            var rawMessage = BuildRawMessage(nameof(SampleEvent), evento);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                dispatcher.DispatchAsync(rawMessage, CancellationToken.None));

            Assert.Equal("Erro de teste", ex.Message);
            logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Erro ao processar evento")),
                ex,
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}