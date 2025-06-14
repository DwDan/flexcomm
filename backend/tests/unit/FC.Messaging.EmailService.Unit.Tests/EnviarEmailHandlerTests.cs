using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.Messaging.EmailService.Application;
using NSubstitute;

namespace FC.Messaging.EmailService.Unit.Tests
{
    public class EnviarEmailHandlerTests
    {
        [Fact(DisplayName = "Deve chamar EnviarAsync ao manipular evento EnviarEmailEvent")]
        [Trait("Email", "EnviarEmailHandler")]
        public async Task HandleAsync_DeveChamarEnviarAsync()
        {
            // Arrange
            var emailSender = Substitute.For<IEmailSender>();
            var handler = new EnviarEmailHandler(emailSender);
            var evento = new EnviarEmailEvent
            {
                To = "teste@teste.com",
                Subject = "Assunto",
                HtmlBody = "<p>Corpo do e-mail</p>"
            };
            var cancellationToken = CancellationToken.None;

            // Act
            await handler.HandleAsync(evento, cancellationToken);

            // Assert
            await emailSender.Received(1).EnviarAsync(
                evento.To,
                evento.Subject,
                evento.HtmlBody,
                cancellationToken);
        }
    }
}
