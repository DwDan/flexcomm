using FC.Auth.Application.Autenticacao.Login;

namespace FC.Auth.Unit.Tests.Application
{
    public class LoginCommandValidatorTests
    {
        [Fact(DisplayName = "Login command validator deve validar com sucesso")]
        [Trait("Autenticação Application", "LoginCommandValidator")]
        public void LoginCommandValidator_DeveValidar_ComSucesso()
        {
            // Arrange
            var command = new LoginCommand()
            {
                Email = "joao@teste.com",
                Senha = "Teste@123",
            };

            // Act
            var result = new LoginCommandValidator().Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Login command validator deve validar com falhas")]
        [Trait("Autenticação Application", "LoginCommandValidator")]
        public void LoginCommandValidator_DeveValidar_ComFalhas()
        {
            // Arrange
            var command = new LoginCommand();

            // Act
            var result = new LoginCommandValidator().Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result!.Errors.Count());
            Assert.Contains(LoginErrors.EmailObrigatorio, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(LoginErrors.SenhaObrigatoria, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
