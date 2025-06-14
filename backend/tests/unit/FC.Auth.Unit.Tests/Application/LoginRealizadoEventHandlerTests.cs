using FC.Auth.Application.Autenticacao.Login;
using FC.Auth.Domain.Messaging.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class LoginRealizadoEventHandlerTests
    {
        [Fact(DisplayName = "Deve logar informações ao receber evento de login realizado")]
        [Trait("Autenticação", "LoginRealizadoEventHandler")]
        public async Task Handle_DeveLogarInformacoesDoLogin()
        {
            // Arrange
            var logger = Substitute.For<ILogger<LoginRealizadoEventHandler>>();
            var handler = new LoginRealizadoEventHandler(logger);
            var evento = new LoginRealizadoEvent(Guid.NewGuid(), "teste@teste.com");

            // Act
            await handler.Handle(evento, CancellationToken.None);

            // Assert
            logger.Received(1).Log(
                LogLevel.Information,
                0,
                Arg.Is<object>(o =>
                    o.ToString().Contains("Login realizado") &&
                    o.ToString().Contains(evento.Email) &&
                    o.ToString().Contains(evento.UsuarioId.ToString())
                ),
                null,
                Arg.Any<Func<object, Exception?, string>>()
            );
        }
    }
}
