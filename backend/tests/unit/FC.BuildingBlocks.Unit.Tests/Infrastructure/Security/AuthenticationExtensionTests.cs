using FC.BuildingBlocks.Domain.Security;
using FC.BuildingBlocks.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.BuildingBlocks.Unit.Infrastructure.Security.Tests
{
    public class AuthenticationExtensionTests
    {
        [Fact(DisplayName = "Deve registrar JwtTokenGenerator e AuthenticationScheme")]
        public void AddJwtAuthentication_ComChaveValida_DeveRegistrarServicos()
        {
            // Arrange
            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string>
            {
                ["Jwt:SecretKey"] = "minha-chave-supersecreta"
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Act
            services.AddJwtAuthentication(configuration);
            var provider = services.BuildServiceProvider();

            // Assert
            var generator = provider.GetService<IJwtTokenGenerator>();
            Assert.NotNull(generator);

            var scheme = provider.GetRequiredService<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>();
            Assert.NotNull(scheme);
        }

        [Fact(DisplayName = "Deve lançar exceção se chave não estiver configurada")]
        public void AddJwtAuthentication_SemChave_DeveLancarExcecao()
        {
            // Arrange
            var services = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()) // vazio
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                services.AddJwtAuthentication(configuration));
        }
    }
}
