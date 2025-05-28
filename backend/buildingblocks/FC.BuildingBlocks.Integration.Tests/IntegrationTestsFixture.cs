using Microsoft.AspNetCore.Mvc.Testing;

namespace FC.BuildingBlocks.Integration.Tests
{
    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public readonly CustomWebApplicationFactory<TProgram> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new CustomWebApplicationFactory<TProgram>();
            Client = Factory.CreateClient(clientOptions);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
