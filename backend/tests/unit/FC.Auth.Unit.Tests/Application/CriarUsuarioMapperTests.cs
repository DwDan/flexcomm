using AutoMapper;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Entities;

namespace FC.Auth.Unit.Tests.Application
{
    public class CriarUsuarioMapperTests
    {
        private readonly IMapper _mapper;

        public CriarUsuarioMapperTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CriarUsuarioProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact(DisplayName = "Deve mapear usuário com sucesso")]
        [Trait("Autenticação", "CriarUsuarioMapperTests")]
        public void DeveMapearUsuario_ComSucesso()
        {
            // Arrange
            var command = new CriarUsuarioCommand()
            {
                Nome = "Teste",
                Email = "teste@teste.com.br",
                Senha = "senhaSimples"
            };

            // Act 
            var result = _mapper.Map<Usuario>(command);

            // Assert
            Assert.NotNull(result);
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
