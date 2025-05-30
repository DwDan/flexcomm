using FC.Auth.Application.Usuarios.CriarUsuario;

namespace FC.Auth.Unit.Tests.Application
{
    public class CriarUsuarioCommandValidatorTests
    {
        [Fact(DisplayName = "Criar usuário command validator deve validar com sucesso")]
        [Trait("Autenticação Application", "CriarUsuarioCommandValidator")]
        public void CriarUsuarioCommand_DeveValidar_ComSucesso()
        {
            // Arrange
            var command = new CriarUsuarioCommand() 
            { 
                Nome = "João",
                Email = "joao@teste.com",
                Senha = "Teste@123",
            };

            // Act
            var result = new CriarUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Criar usuário command validator deve validar com falhas")]
        [Trait("Autenticação Application", "CriarUsuarioCommandValidator")]
        public void CriarUsuarioCommand_DeveValidar_ComFalhas()
        {
            // Arrange
            var command = new CriarUsuarioCommand() 
            { 
                Nome = "",
                Email = "",
                Senha = "",
            };

            // Act
            var result = new CriarUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result);
            Assert.Equal(10, result!.Errors.Count());
            Assert.Contains(CriarUsuarioCommandValidator.NomeObrigatorio, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.EmailObrigatorio, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaObrigatoria, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.NomeInvalido, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.EmailInvalido, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaTamanhoCaracteres, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaLetraMaiuscula, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaLetraMinuscula, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaNumero, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaCaracter, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
