using FC.BuildingBlocks.Infrastructure.Messaging.EventBus;
using FluentValidation.TestHelper;

namespace FC.BuildingBlocks.Unit.Infrastructure.Messaging.EventBus.Tests
{
    public class EventBusConsumerSettingsValidatorTests
    {
        private readonly EventBusConsumerSettingsValidator _validator = new();

        [Fact(DisplayName = "Deve falhar se Topic estiver vazio")]
        public void DeveValidarTopicObrigatorio()
        {
            var settings = new EventBusConsumerSettings
            {
                Topic = ""
            };

            var result = _validator.TestValidate(settings);

            result.ShouldHaveValidationErrorFor(x => x.Topic)
                  .WithErrorMessage("Configuração EventBus.Topic obrigatória.");
        }

        [Fact(DisplayName = "Deve passar se Topic estiver preenchido")]
        public void DeveValidarComSucesso()
        {
            var settings = new EventBusConsumerSettings
            {
                Topic = "authorization"
            };

            var result = _validator.TestValidate(settings);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}