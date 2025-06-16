using FC.Auth.Application.Usuarios.AtivarUsuario;

namespace FC.Auth.Unit.Tests.Application
{
    public class AtivarUsuarioCommandHandlerValidatorTests
    {
        [Fact(DisplayName = "Ativar usuário command validator deve validar com sucesso")]
        [Trait("Autenticação Application", "AtivarUsuarioCommandHandlerValidator")]
        public void AtivarUsuarioCommand_DeveValidar_ComSucesso()
        {
            // Arrange
            var command = new AtivarUsuarioCommand()
            {
                Id = Guid.NewGuid(),
            };

            // Act
            var result = new AtivarUsuarioCommandHandlerValidator().Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Inativar usuário command validator deve validar com falhas")]
        [Trait("Autenticação Application", "AtivarUsuarioCommandHandlerValidator")]
        public void AtivarUsuarioCommand_DeveValidar_ComFalhas()
        {
            // Arrange
            var command = new AtivarUsuarioCommand()
            {
                Id = Guid.Empty
            };

            // Act
            var result = new AtivarUsuarioCommandHandlerValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(AtivarUsuarioCommandHandlerValidator.AtivarUsuario_IdInvalido, result.Errors.Select(e => e.ErrorCode));
        }
    }
}
