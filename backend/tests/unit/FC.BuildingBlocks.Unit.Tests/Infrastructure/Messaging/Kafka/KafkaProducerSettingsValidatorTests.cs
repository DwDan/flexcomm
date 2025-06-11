using FC.BuildingBlocks.Infrastructure.Messaging.Kafka;
using FluentValidation.TestHelper;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.Kafka.Tests
{
    public class KafkaProducerSettingsValidatorTests
    {
        private readonly KafkaProducerSettingsValidator _validator = new();

        [Fact(DisplayName = "Deve falhar se BootstrapServers estiver vazio")]
        public void DeveValidarBootstrapServersObrigatorio()
        {
            var settings = new KafkaProducerSettings
            {
                BootstrapServers = "",
                ClientId = "fc-auth-service"
            };

            var result = _validator.TestValidate(settings);

            result.ShouldHaveValidationErrorFor(x => x.BootstrapServers)
                  .WithErrorMessage("Configuração EventBus.BootstrapServers obrigatória.");
        }

        [Fact(DisplayName = "Deve falhar se ClientId estiver vazio")]
        public void DeveValidarClientIdObrigatorio()
        {
            var settings = new KafkaProducerSettings
            {
                BootstrapServers = "localhost:9092",
                ClientId = ""
            };

            var result = _validator.TestValidate(settings);

            result.ShouldHaveValidationErrorFor(x => x.ClientId)
                  .WithErrorMessage("Configuração EventBus.ClientId obrigatória.");
        }

        [Fact(DisplayName = "Deve passar se configurações estiverem válidas")]
        public void DeveValidarComSucesso()
        {
            var settings = new KafkaProducerSettings
            {
                BootstrapServers = "localhost:9092",
                ClientId = "fc-auth-service"
            };

            var result = _validator.TestValidate(settings);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
