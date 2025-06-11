using FC.BuildingBlocks.Infrastructure.Messaging.Kafka;
using FluentValidation.TestHelper;

namespace FC.BuildingBlocks.Unit.Tests.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerSettingsValidatorTests
    {
        private readonly KafkaConsumerSettingsValidator _validator = new();

        [Fact(DisplayName = "Deve falhar se BootstrapServers estiver vazio")]
        public void DeveValidarBootstrapServers()
        {
            var model = new KafkaConsumerSettings
            {
                BootstrapServers = "",
                Topic = "meu-topico",
                GroupId = "meu-grupo"
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.BootstrapServers)
                  .WithErrorMessage("Configuração EventBus.BootstrapServers obrigatória.");
        }

        [Fact(DisplayName = "Deve falhar se Topic estiver vazio")]
        public void DeveValidarTopic()
        {
            var model = new KafkaConsumerSettings
            {
                BootstrapServers = "localhost:9092",
                Topic = "",
                GroupId = "meu-grupo"
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Topic)
                  .WithErrorMessage("Configuração EventBus.Topic obrigatória.");
        }

        [Fact(DisplayName = "Deve falhar se GroupId estiver vazio")]
        public void DeveValidarGroupId()
        {
            var model = new KafkaConsumerSettings
            {
                BootstrapServers = "localhost:9092",
                Topic = "meu-topico",
                GroupId = ""
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.GroupId)
                  .WithErrorMessage("Configuração EventBus.GroupId obrigatória.");
        }

        [Fact(DisplayName = "Deve passar se todas as configurações estiverem preenchidas")]
        public void DeveValidarComSucesso()
        {
            var model = new KafkaConsumerSettings
            {
                BootstrapServers = "localhost:9092",
                Topic = "meu-topico",
                GroupId = "meu-grupo",
                ClientId = "meu-client-id",
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
