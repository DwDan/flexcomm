using FC.Auth.Application.Usuarios.InativarUsuario;
using FC.Auth.Domain.Messaging.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class UsuarioAtivadoEventHandlerTests
    {
        [Fact(DisplayName = "Deve logar informações ao receber evento de usuário ativado")]
        [Trait("Autenticação", "UsuarioAtivadoEventHandler")]
        public async Task Handle_DeveLogarInformacoesDoUsuarioAtivado()
        {
            // Arrange
            var logger = Substitute.For<ILogger<UsuatioAtivadoEventHandler>>();
            var handler = new UsuatioAtivadoEventHandler(logger);
            var usuarioId = Guid.NewGuid();
            var evento = new UsuarioAtivadoEvent(usuarioId, "João", "joao@teste.com");

            // Act
            await handler.Handle(evento, CancellationToken.None);

            // Assert
            logger.Received(1).Log(
                LogLevel.Information,
                0,
                Arg.Is<object>(o => o.ToString().Contains("Usuário ativado") && o.ToString().Contains(usuarioId.ToString()) && o.ToString().Contains("joao@teste.com")),
                null,
                Arg.Any<Func<object, Exception?, string>>()
            );
        }
    }
}