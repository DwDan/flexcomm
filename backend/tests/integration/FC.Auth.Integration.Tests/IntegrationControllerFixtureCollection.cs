using FC.Auth.Infrastructure;
using FC.Auth.WebAPI;
using FC.BuildingBlocks.Integration.Tests;

namespace FC.Auth.Integration.Tests
{
    public class AuthIntegrationTestsFixture : IntegrationTestsFixture<Program, AutenticacaoContext>
    {
        public AuthIntegrationTestsFixture() : base("FC.Auth.Infrastructure") { }
    }

    [CollectionDefinition(nameof(IntegrationControllerFixtureCollection))]
    public class IntegrationControllerFixtureCollection : ICollectionFixture<AuthIntegrationTestsFixture> { }
}
