using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Domain.Messaging.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class UsuarioAlteradoEventHandlerTests
    {
        [Fact(DisplayName = "Deve logar informações ao receber evento de usuário alterado")]
        [Trait("Autenticação", "UsuarioAlteradoEventHandler")]
        public async Task Handle_DeveLogarInformacoesDoUsuarioAlterado()
        {
            // Arrange
            var logger = Substitute.For<ILogger<UsuarioAlteradoEventHandler>>();
            var handler = new UsuarioAlteradoEventHandler(logger);
            var evento = new UsuarioAlteradoEvent(Guid.NewGuid(), "João", "joao@teste.com");

            // Act
            await handler.Handle(evento, CancellationToken.None);

            // Assert
            logger.Received(1).Log(
                LogLevel.Information,
                0,
                Arg.Is<object>(o => o.ToString().Contains("Usuário alterado") && o.ToString().Contains("João") && o.ToString().Contains("joao@teste.com")),
                null,
                Arg.Any<Func<object, Exception?, string>>()
            );
        }
    }
}