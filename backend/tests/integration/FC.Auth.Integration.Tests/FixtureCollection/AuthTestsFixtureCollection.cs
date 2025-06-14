using FC.Auth.Infrastructure;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.BuildingBlocks.Domain.Security;
using FC.BuildingBlocks.Integration.Tests.Fixture;
using FC.BuildingBlocks.Integration.Tests.Handler;
using FC.BuildingBlocks.Integration.Tests.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FC.Auth.Integration.Tests
{
    public class AuthTestsFixture : IntegrationEndToEndFixture
    {
        public TestEmailHandler EmailHandler { get; }
        public UsuarioTestHelper UsuarioTestHelper { get; }
        public IJwtTokenGenerator JwtTokenGenerator { get; }

        public AuthTestsFixture() : base()
        {
            EmailHandler = new TestEmailHandler();

            WithEventBus();

            WithTopic("email-topic");

            WithProducer<FC.Auth.WebAPI.Program, AutenticacaoContext>(
                name: "Auth",
                migrationsAssembly: "FC.Auth.Infrastructure");

            WithWorker<FC.Messaging.EmailService.Program>(
                name: "EmailService",
                isProducer: false,
                isConsumer: true,
                configure: services =>
                {
                    services.RemoveAll(typeof(IIntegrationEventHandler<EnviarEmailEvent>));
                    services.AddSingleton<IIntegrationEventHandler<EnviarEmailEvent>>(EmailHandler);
                });

            UsuarioTestHelper = new UsuarioTestHelper(Clients["Auth"]);

            JwtTokenGenerator = GetFactory<FC.Auth.WebAPI.Program>("Auth")
                .Services.CreateScope()
                .ServiceProvider.GetRequiredService<IJwtTokenGenerator>();
        }
    }

    [CollectionDefinition(nameof(AuthTestsFixtureCollection))]
    public class AuthTestsFixtureCollection : ICollectionFixture<AuthTestsFixture> { }
}
