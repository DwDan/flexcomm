using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Enums;
using FC.Auth.Domain.Validation;

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
                Senha = "hashed_password",
                Perfil = PerfilUsuario.Client
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

            Assert.Equal(6, result.Errors.Count());
            Assert.Contains(UsuarioValidation.NomeObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.EmailObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.SenhaObrigatoria, result.Errors.Select(c => c.ErrorMessage));

            Assert.Contains(UsuarioValidation.NomeInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.EmailInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.SenhaInvalida, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
