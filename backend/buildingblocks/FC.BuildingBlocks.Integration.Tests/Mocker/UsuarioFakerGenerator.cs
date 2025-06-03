using Bogus;

namespace FC.BuildingBlocks.Integration.Tests.Mocker
{
    public class FakeUsuarioGenerator
    {
        private readonly Faker _faker;

        public FakeUsuarioGenerator()
        {
            _faker = new Faker("pt_BR");
        }

        public Dictionary<string, string> Gerar()
        {
            var nome = _faker.Name.FullName();
            var email = $"usuario_{Guid.NewGuid()}@teste.com";
            var senha = "Usuario-Teste@123";

            return new Dictionary<string, string>
            {
                { "Nome", nome },
                { "Email", email },
                { "Senha", senha }
            };
        }
    }
}
