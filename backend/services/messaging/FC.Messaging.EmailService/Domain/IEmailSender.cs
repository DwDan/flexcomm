namespace FC.Messaging.EmailService.Domain
{
    public interface IEmailSender
    {
        Task EnviarAsync(string destinatario, string assunto, string corpoHtml, CancellationToken cancellationToken = default);
    }
}
