using Bogus;
using FC.BuildingBlocks.Integration.Tests.DTO;

namespace FC.BuildingBlocks.Integration.Tests.Mocker
{
    public class FakeUsuarioGenerator
    {
        private readonly Faker _faker;

        public FakeUsuarioGenerator()
        {
            _faker = new Faker("pt_BR");
        }

        public UsuarioDto Gerar()
        {
            return new UsuarioDto() 
            { 
                Nome = _faker.Name.FullName(),
                Email = $"usuario_{Guid.NewGuid()}@teste.com",
                Senha = "Usuario-Teste@123"
            };
        }
    }
}
