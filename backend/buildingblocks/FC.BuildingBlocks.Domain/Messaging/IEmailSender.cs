namespace FC.BuildingBlocks.Domain.Messaging
{
    public interface IEmailSender
    {
        Task EnviarAsync(string destinatario, string assunto, string corpoHtml, CancellationToken cancellationToken = default);
    }
}
