using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Application.Usuarios.AlterarUsuario.DTO;

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
                Endereco = new AlterarUsuarioEnderecoDto()
                {
                    Bairro = "Bairro Teste",
                    Cidade = "Cidade Teste",
                    Estado = "Estado Teste",
                    Numero = "123",
                    Logradouro = "Logradouro Teste",
                    Cep = "12345-678"
                },
                NomeCompleto = new AlterarUsuarioNomeCompletoDto()
                {
                    PrimeiroNome = "Primeiro Nome Teste",
                    UltimoNome = "Ultimo Nome Teste"
                },
                Telefone = new AlterarUsuarioNumeroTelefoneDto()
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
                Endereco = new AlterarUsuarioEnderecoDto(),
                NomeCompleto = new AlterarUsuarioNomeCompletoDto(),
                Telefone = new AlterarUsuarioNumeroTelefoneDto()
            };

            // Act
            var result = new AlterarUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result);
            Assert.Equal(15, result.Errors.Count());
            Assert.Contains("AlterarUsuario.NomeObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NomeInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.SobrenomeObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.SobrenomeInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.LogradouroObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NumeroObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.BairroObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.CidadeObrigatoria", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.EstadoObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.CepObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.CepInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.DddObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.DddInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NumeroTelefoneObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NumeroTelefoneInvalido", result.Errors.Select(c => c.ErrorCode));
        }
    }
}
