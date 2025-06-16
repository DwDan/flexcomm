using FC.Auth.Application.Usuarios.InativarUsuario;
using FC.Auth.Domain.Messaging.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class UsuarioInativadoEventHandlerTests
    {
        [Fact(DisplayName = "Deve logar informações ao receber evento de usuário inativado")]
        [Trait("Autenticação", "UsuarioInativadoEventHandlerTests")]
        public async Task Handle_DeveLogarInformacoesDoUsuarioInativado()
        {
            // Arrange
            var logger = Substitute.For<ILogger<UsuarioInativadoEventHandler>>();
            var handler = new UsuarioInativadoEventHandler(logger);
            var usuarioId = Guid.NewGuid();
            var evento = new UsuarioInativadoEvent(usuarioId, "João", "joao@teste.com");

            // Act
            await handler.Handle(evento, CancellationToken.None);

            // Assert
            logger.Received(1).Log(
                LogLevel.Information,
                0,
                Arg.Is<object>(o => o.ToString().Contains("Usuário inativado") && o.ToString().Contains(usuarioId.ToString()) && o.ToString().Contains("joao@teste.com")),
                null,
                Arg.Any<Func<object, Exception?, string>>()
            );
        }
    }
}