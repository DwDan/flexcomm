using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.WebAPI.Tests
{
    public class CorrelationIdMiddlewareTests
    {
        [Fact(DisplayName = "Deve gerar novo correlationId se header não estiver presente")]
        public async Task SemHeader_DeveGerarCorrelationId()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var correlationContext = Substitute.For<ICorrelationContext>();

            context.RequestServices = new ServiceCollection()
                .AddSingleton(correlationContext)
                .BuildServiceProvider();

            var logger = Substitute.For<ILogger<CorrelationIdMiddleware>>();

            var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, logger);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Guid correlationId;
            correlationContext.Received(1).Set(Arg.Is<string>(id => Guid.TryParse(id, out correlationId)));
            Assert.True(context.Response.Headers.ContainsKey("X-Correlation-ID"));
        }

        [Fact(DisplayName = "Deve utilizar CorrelationId presente no header")]
        public async Task ComHeader_DeveUsarCorrelationId()
        {
            // Arrange
            var correlationId = Guid.NewGuid().ToString();
            var context = new DefaultHttpContext();
            context.Request.Headers["X-Correlation-ID"] = correlationId;

            var correlationContext = Substitute.For<ICorrelationContext>();
            context.RequestServices = new ServiceCollection()
                .AddSingleton(correlationContext)
                .BuildServiceProvider();

            var logger = Substitute.For<ILogger<CorrelationIdMiddleware>>();

            var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, logger);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            correlationContext.Received(1).Set(correlationId);
            Assert.Equal(correlationId, context.Response.Headers["X-Correlation-ID"]);
        }

        [Fact(DisplayName = "Deve logar erro se exceção for lançada no pipeline")]
        public async Task ExcecaoDurantePipeline_DeveLogarErro()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var correlationContext = Substitute.For<ICorrelationContext>();

            context.RequestServices = new ServiceCollection()
                .AddSingleton(correlationContext)
                .BuildServiceProvider();

            var logger = Substitute.For<ILogger<CorrelationIdMiddleware>>();

            var middleware = new CorrelationIdMiddleware(_ => throw new InvalidOperationException("Teste"), logger);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(context));

            logger.Received(1).Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Erro durante requisição")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}
