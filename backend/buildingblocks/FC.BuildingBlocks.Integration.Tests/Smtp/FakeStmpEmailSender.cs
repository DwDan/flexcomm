using FC.BuildingBlocks.Domain.Messaging;

namespace FC.BuildingBlocks.Integration.Tests.Smtp
{
    public class FakeStmpEmailSender : IEmailSender
    {
        public bool FoiExecutado { get; private set; }

        public Task EnviarAsync(string destinatario, string assunto, string corpoHtml, CancellationToken cancellationToken = default)
        {
            FoiExecutado = true;

            return Task.CompletedTask;
        }
    }
}
