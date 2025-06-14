using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Messaging.Events;
using FC.BuildingBlocks.Application;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.BuildingBlocks.Domain.Security;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class UsuarioCriadoEventHandlerTests
    {
        [Fact(DisplayName = "Deve publicar evento de e-mail ao criar usuário")]
        [Trait("Autenticação", "UsuarioCriadoEventHandler")]
        public async Task Handle_DevePublicarEventoEmailConfirmacao()
        {
            // Arrange
            var eventProducer = Substitute.For<IEventBusProducer>();
            var jwtGenerator = Substitute.For<IJwtTokenGenerator>();
            var options = Options.Create(new AplicacaoSettings { UrlBase = "http://localhost" });

            var tokenEsperado = "fake-token";
            jwtGenerator.GenerateTokenEmailConfirmation(Arg.Any<UsuarioCriadoEvent>()).Returns(tokenEsperado);

            var handler = new UsuarioCriadoEventHandler(eventProducer, jwtGenerator, options);
            var evento = new UsuarioCriadoEvent(Guid.NewGuid(), "usuario@teste.com");

            // Act
            await handler.Handle(evento, CancellationToken.None);

            // Assert
            await eventProducer.Received(1).PublicarAsync("email-topic",
                Arg.Is<EnviarEmailEvent>(email =>
                    email.To == "usuario@teste.com" &&
                    email.Subject == "Bem-vindo!" &&
                    email.HtmlBody.Contains("http://localhost/api/usuario/confirmar-email?token=fake-token")
                ));
        }
    }
}
