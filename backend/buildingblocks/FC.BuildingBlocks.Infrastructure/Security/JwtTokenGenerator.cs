using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FC.BuildingBlocks.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FC.BuildingBlocks.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GenerateTokenInternal(IEnumerable<Claim> claims, DateTime expires, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(IUsuario user)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Active", user.Ativo.ToString()),
                new Claim("ConfirmedEmail", user.EmailConfirmado.ToString())
            };

            return GenerateTokenInternal(claims, DateTime.UtcNow.AddHours(8), secretKey);
        }

        public string GenerateTokenEmailConfirmation(IUsuario user)
        {
            var secretKey = _configuration["Jwt:EmailConfirmationSecret"];
            ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("action", "confirm_email")
            };

            return GenerateTokenInternal(claims, DateTime.UtcNow.AddMinutes(15), secretKey);
        }
    }
}
