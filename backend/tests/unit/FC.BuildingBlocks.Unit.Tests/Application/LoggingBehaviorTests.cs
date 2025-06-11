using FC.BuildingBlocks.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Application.Tests
{
    public class LoggingBehaviorTests
    {
        public record TestRequest(string Mensagem) : IRequest<string>;

        [Fact(DisplayName = "Deve logar início e fim do handler")]
        public async Task Handle_DeveLogarInicioEFim()
        {
            // Arrange
            var logger = Substitute.For<ILogger<LoggingBehavior<TestRequest, string>>>();

            var behavior = new LoggingBehavior<TestRequest, string>(logger);

            var request = new TestRequest("Olá");

            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next.Invoke().Returns("ok");

            // Act
            var response = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.Equal("ok", response);

            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(msg => msg.ToString()!.Contains("Handling TestRequest")),
                null,
                Arg.Any<Func<object, Exception?, string>>());

            logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(msg => msg.ToString()!.Contains("Handled TestRequest")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}
