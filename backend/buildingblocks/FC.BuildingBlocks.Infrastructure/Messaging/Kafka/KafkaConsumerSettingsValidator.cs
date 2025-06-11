using FluentValidation;

namespace FC.BuildingBlocks.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerSettingsValidator : AbstractValidator<KafkaConsumerSettings>
    {
        public KafkaConsumerSettingsValidator()
        {
            RuleFor(x => x.BootstrapServers)
                .NotEmpty().WithMessage("Configuração EventBus.BootstrapServers obrigatória.");

            RuleFor(x => x.Topic)
                .NotEmpty().WithMessage("Configuração EventBus.Topic obrigatória.");

            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("Configuração EventBus.GroupId obrigatória.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Configuração EventBus.ClientId obrigatória.");
        }
    }
}
