using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Messaging.EventBus;

namespace FC.Messaging.EmailService.Infrastructure
{
    public class MessageConsumerService : BackgroundService, IEventBackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MessageConsumerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IEventBusConsumer>();

            await consumer.StartAsync(stoppingToken);
        }
    }
}