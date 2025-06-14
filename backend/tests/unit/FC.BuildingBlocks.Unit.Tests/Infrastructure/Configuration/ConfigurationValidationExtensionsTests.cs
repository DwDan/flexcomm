using FC.BuildingBlocks.Infrastructure.Configuration;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.BuildingBlocks.Unit.Infrastructure.Configuration.Tests
{
    public class ConfigurationValidationExtensionsTests
    {
        private class TestSettings
        {
            public string Valor { get; set; } = string.Empty;
        }

        private class TestSettingsValidator : AbstractValidator<TestSettings>
        {
            public TestSettingsValidator()
            {
                RuleFor(x => x.Valor).NotEmpty().WithMessage("Valor é obrigatório.");
            }
        }

        [Fact(DisplayName = "EnsureSettings deve registrar configurações válidas com sucesso")]
        public void EnsureSettings_ConfiguracaoValida_DeveRegistrar()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                ["MinhaSecao:Valor"] = "algum valor"
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();

            // Act & Assert (não deve lançar exceção)
            services.EnsureSettings<TestSettings, TestSettingsValidator>(configuration, "MinhaSecao");
        }

        [Fact(DisplayName = "EnsureSettings deve lançar exceção se configuração estiver ausente")]
        public void EnsureSettings_ConfiguracaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var configuration = new ConfigurationBuilder().Build();
            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                services.EnsureSettings<TestSettings, TestSettingsValidator>(configuration, "SecaoInvalida"));

            Assert.Contains("SecaoInvalida config não encontrada", ex.Message);
        }

        [Fact(DisplayName = "EnsureSettings deve lançar exceção se configuração for inválida")]
        public void EnsureSettings_ConfiguracaoInvalida_DeveLancarExcecao()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                ["MinhaSecao:Valor"] = "" // Inválido (vazio)
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                services.EnsureSettings<TestSettings, TestSettingsValidator>(configuration, "MinhaSecao"));

            Assert.Contains("Valor é obrigatório", ex.Message);
        }
    }
}
