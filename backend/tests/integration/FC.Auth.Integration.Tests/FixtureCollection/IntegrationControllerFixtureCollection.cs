using FC.Auth.Infrastructure;
using FC.BuildingBlocks.Domain.Security;
using FC.BuildingBlocks.Integration.Tests.Fixture;
using FC.BuildingBlocks.Integration.Tests.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Auth.Integration.Tests.FixtureCollection
{
    public class AuthIntegrationTestsFixture : IntegrationEndToEndFixture
    {
        public UsuarioTestHelper UsuarioTestHelper { get; }
        public IJwtTokenGenerator JwtTokenGenerator { get; }

        public AuthIntegrationTestsFixture() : base()
        {
            WithEventBus();

            WithProducer<FC.Auth.WebAPI.Program, AutenticacaoContext>(
                name: "Auth",
                migrationsAssembly: "FC.Auth.Infrastructure");

            UsuarioTestHelper = new UsuarioTestHelper(Clients["Auth"]);

            JwtTokenGenerator = GetFactory<FC.Auth.WebAPI.Program>("Auth")
                .Services.CreateScope()
                .ServiceProvider.GetRequiredService<IJwtTokenGenerator>();
        }
    }

    [CollectionDefinition(nameof(IntegrationControllerFixtureCollection))]
    public class IntegrationControllerFixtureCollection : ICollectionFixture<AuthIntegrationTestsFixture> { }
}
