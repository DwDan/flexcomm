using FluentValidation;

namespace FC.BuildingBlocks.Infrastructure.Messaging.Kafka
{
    public class KafkaProducerSettingsValidator : AbstractValidator<KafkaProducerSettings>
    {
        public KafkaProducerSettingsValidator()
        {
            RuleFor(x => x.BootstrapServers)
                .NotEmpty().WithMessage("Configuração EventBus.BootstrapServers obrigatória.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Configuração EventBus.ClientId obrigatória.");
        }
    }
}
