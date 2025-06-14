using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain.Security;
using FC.BuildingBlocks.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Tests
{
    public class EmailConfirmationTokenValidatorTests
    {
        private const string SecretKey = "minha-chave-super-secreta-para-testes";

        [Fact(DisplayName = "Validate Deve Retornar UserId Quando Token É Válido")]
        public void Validate_DeveRetornarUserId_QuandoTokenValido()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:EmailConfirmationSecret"].Returns(SecretKey);

            var tokenGenerator = new JwtTokenGenerator(configuration);
            var validator = new EmailConfirmationTokenValidator(configuration);

            var userId = Guid.NewGuid();
            var usuario = Substitute.For<IUsuario>();
            usuario.Id.Returns(userId);
            usuario.Email.Returns("email@teste.com");
            var token = tokenGenerator.GenerateTokenEmailConfirmation(usuario);

            // Act
            var result = validator.Validate(token);

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact(DisplayName = "Validate Deve Lançar Exceção Quando Action É Inválida")]
        public void Validate_DeveLancarExcecao_QuandoActionInvalida()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:EmailConfirmationSecret"].Returns(SecretKey);

            var validator = new EmailConfirmationTokenValidator(configuration);
            var token = GenerateToken(Guid.NewGuid(), "email@teste.com", action: "wrong_action");

            // Act & Assert
            Assert.Throws<TokenSecurityException>(() => validator.Validate(token));
        }

        [Fact(DisplayName = "Validate Deve Lançar Exceção Quando Claims São Inválidas")]
        public void Validate_DeveLancarExcecao_QuandoClaimsInvalidas()
        {
            // Arrange
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, "email@teste.com"),
                new Claim("action", "confirm_email")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(handler.CreateToken(tokenDescriptor));

            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:EmailConfirmationSecret"].Returns(SecretKey);

            var validator = new EmailConfirmationTokenValidator(configuration);

            // Act & Assert
            Assert.Throws<TokenSecurityException>(() => validator.Validate(token));
        }

        private string GenerateToken(Guid userId, string email, string action)
        {
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("action", action)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(tokenDescriptor));
        }
    }
}