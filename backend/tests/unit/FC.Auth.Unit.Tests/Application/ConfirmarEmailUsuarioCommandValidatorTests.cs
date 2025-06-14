using FC.Auth.Application.Usuarios.ConfirmarEmail;

namespace FC.Auth.Unit.Tests.Application
{
    public class ConfirmarEmailUsuarioCommandValidatorTests
    {
        [Fact(DisplayName = "Confirmar email command validator deve validar com sucesso")]
        [Trait("Autenticação Application", "ConfirmarEmailUsuarioCommandValidatorTests")]
        public void ConfirmarEmailUsuarioCommand_DeveValidar_ComSucesso()
        {
            // Arrange
            var command = new ConfirmarEmailUsuarioCommand("valid-token");

            // Act
            var result = new ConfirmarEmailUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Confirmar email command com token vazio validator deve validar com falhas")]
        [Trait("Autenticação Application", "ConfirmarEmailUsuarioCommandValidator")]
        public void ConfirmarEmailUsuarioCommand_TokenVazio_DeveValidar_ComFalhas()
        {
            // Arrange
            var command = new ConfirmarEmailUsuarioCommand(string.Empty);

            // Act
            var result = new ConfirmarEmailUsuarioCommandValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result);
            Assert.Contains(ConfirmarEmailUsuarioCommandValidator.ConfirmacaoEmail_TokenObrigatorio, result.Errors.Select(c => c.ErrorCode));
        }
    }
}
