using FC.Auth.Application.Usuarios.AlterarUsuario;

namespace FC.Auth.Unit.Tests.Application
{
    public class AlterarUsuarioCommandValidatorTests
    {
        [Fact(DisplayName = "Alterar usuário command validator deve validar com sucesso")]
        [Trait("Autenticação Application", "AlterarUsuarioCommandValidator")]
        public void AlterarUsuarioCommand_DeveValidar_ComSucesso()
        {
            // Arrange
            var command = new AlterarUsuarioCommand()
            {
                Id = Guid.NewGuid(),
                Endereco = new AlterarUsuarioEndereco()
                {
                    Bairro = "Bairro Teste",
                    Cidade = "Cidade Teste",
                    Estado = "Estado Teste",
                    Numero = "123",
                    Logradouro = "Logradouro Teste",
                    Cep = "12345-678"
                },
                NomeCompleto = new AlterarUsuarioNomeCompleto()
                {
                    PrimeiroNome = "Primeiro Nome Teste",
                    UltimoNome = "Ultimo Nome Teste"
                },
                Telefone = new AlterarUsuarioNumeroTelefone()
                {
                    Ddd = "11",
                    Numero = "987654321"
                }
            };

            // Act
            var result = new AlterarUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Alterar usuário command validator deve validar com falhas")]
        [Trait("Autenticação Application", "AlterarUsuarioCommandValidator")]
        public void AlterarUsuarioCommand_DeveValidar_ComFalhas()
        {
            // Arrange
            var command = new AlterarUsuarioCommand()
            {
                Id = Guid.NewGuid(),
                Endereco = new AlterarUsuarioEndereco(),
                NomeCompleto = new AlterarUsuarioNomeCompleto(),
                Telefone = new AlterarUsuarioNumeroTelefone()
            };

            // Act
            var result = new AlterarUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result);
            Assert.Equal(15, result.Errors.Count());
            Assert.Contains(AlterarUsuarioCommandValidator.NomeObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.NomeInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.SobrenomeObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.SobrenomeInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.LogradouroObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.NumeroObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.BairroObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.CidadeObrigatoria, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.EstadoObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.CepObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.CepInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.DddObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.DddInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.NumeroTelefoneObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AlterarUsuarioCommandValidator.NumeroTelefoneInvalido, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
