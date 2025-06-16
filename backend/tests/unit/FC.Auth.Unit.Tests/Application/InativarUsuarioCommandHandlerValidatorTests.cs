using FC.Auth.Application.Usuarios.InativarUsuario;

namespace FC.Auth.Unit.Tests.Application
{
    public class InativarUsuarioCommandHandlerValidatorTests
    {
        [Fact(DisplayName = "Inativar usuário command validator deve validar com sucesso")]
        [Trait("Autenticação Application", "InativarUsuarioCommandHandlerValidator")]
        public void AtivarUsuarioCommand_DeveValidar_ComSucesso()
        {
            // Arrange
            var command = new InativarUsuarioCommand()
            {
                Id = Guid.NewGuid(),
            };

            // Act
            var result = new InativarUsuarioCommandHandlerValidator().Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Inativar usuário command validator deve validar com falhas")]
        [Trait("Autenticação Application", "InativarUsuarioCommandHandlerValidator")]
        public void AtivarUsuarioCommand_DeveValidar_ComFalhas()
        {
            // Arrange
            var command = new InativarUsuarioCommand()
            {
                Id = Guid.Empty
            };

            // Act
            var result = new InativarUsuarioCommandHandlerValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(InativarUsuarioCommandHandlerValidator.InativarUsuario_IdInvalido, result.Errors.Select(e => e.ErrorCode));
        }
    }
}
