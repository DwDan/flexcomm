using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.Events;

namespace FC.BuildingBlocks.Integration.Tests.Handler
{
    public class TestEmailHandler : IIntegrationEventHandler<EnviarEmailEvent>
    {
        public bool FoiExecutado { get; private set; }

        public Task HandleAsync(EnviarEmailEvent @event, CancellationToken cancellationToken)
        {
            FoiExecutado = true;

            return Task.CompletedTask;
        }
    }
}
