using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FC.BuildingBlocks.Infrastructure.Security
{
    public class EmailConfirmationTokenValidator : IEmailConfirmationTokenValidator
    {
        private readonly IConfiguration _configuration;

        public EmailConfirmationTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Guid Validate(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _configuration["Jwt:EmailConfirmationSecret"];
            ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

            var key = Encoding.ASCII.GetBytes(secretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out _);

            var action = principal.FindFirst("action")?.Value;
            if (action != "confirm_email")
                throw new TokenSecurityException("ConfirmacaoEmail.TokenInvalido");

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId) || string.IsNullOrWhiteSpace(emailClaim))
                throw new TokenSecurityException("ConfirmacaoEmail.ClaimsInvalidas");

            return userId;
        }
    }
}
