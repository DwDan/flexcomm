using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Enums;
using FC.Auth.Domain.Validation;

namespace FC.Auth.Unit.Tests
{
    public class UsuarioEntityTests
    {
        [Fact(DisplayName = "Cria usuário deve criar usuário ativo")]
        [Trait("Categoria", "Autenticação")]
        public void CriarUsuario_DeveCriar_UsuarioAtivo()
        {
            // Arrange
            var user = new Usuario("João", "joao@teste.com", "hashed_password", UserRole.Client);

            // Act & Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("joao@teste.com", user.Email);
            Assert.Equal("João", user.Name);
            Assert.Equal("hashed_password", user.PasswordHash);
            Assert.Equal(UserRole.Client, user.Role);
            Assert.True(user.Active);
        }

        [Fact(DisplayName = "Cria usuário deve criar usuário válido")]
        [Trait("Categoria", "Autenticação")]
        public void CriarUsuario_DeveCriar_Valido()
        {
            // Arrange
            var user = new Usuario("João", "joao@teste.com", "hashed_password", UserRole.Client);

            // Act
            var result = user.Validar();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Cria usuário deve criar usuário inválido")]
        [Trait("Categoria", "Autenticação")]
        public void CriarUsuario_DeveCriar_UsuarioInvalido()
        {
            // Arrange
            var user = new Usuario(string.Empty, string.Empty, string.Empty, UserRole.Client);

            // Act
            var result = user.Validar();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(UsuarioValidation.NomeObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.EmailObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.SenhaObrigatoria, result.Errors.Select(c => c.ErrorMessage));

            Assert.Contains(UsuarioValidation.NomeInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.EmailInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.SenhaInvalida, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
