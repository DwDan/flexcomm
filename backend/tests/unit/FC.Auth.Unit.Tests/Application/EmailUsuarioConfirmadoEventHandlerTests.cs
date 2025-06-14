using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Messaging.Events;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class EmailUsuarioConfirmadoEventHandlerTests
    {
        [Fact(DisplayName = "Deve publicar evento de e-mail ao confirmar e-mail do usuário")]
        [Trait("Autenticação", "EmailUsuarioConfirmadoEventHandler")]
        public async Task Handle_DevePublicarEventoDeEmail()
        {
            // Arrange
            var eventProducer = Substitute.For<IEventBusProducer>();
            var handler = new EmailUsuarioConfirmadoEventHandler(eventProducer);
            var evento = new EmailUsuarioConfirmadoEvent("João", "joao@teste.com");

            // Act
            await handler.Handle(evento, CancellationToken.None);

            // Assert
            await eventProducer.Received(1).PublicarAsync("email-topic",
                Arg.Is<EnviarEmailEvent>(email =>
                    email.To == "joao@teste.com" &&
                    email.Subject == "Confirmação de E-mail" &&
                    email.HtmlBody.Contains("Olá, João!") &&
                    email.HtmlBody.Contains("foi confirmado com sucesso")
                ));
        }
    }
}