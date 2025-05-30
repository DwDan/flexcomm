using FC.BuildingBlocks.Infrastructure.Security;

namespace FC.BuildingBlocks.Unit.Tests
{
    public class BCryptPasswordHashTests
    {
        [Fact(DisplayName = "Hash Password Deve Gerar Hash Valido")]
        [Trait("Autenticação", "BCryptPasswordHashTests")]
        public void HashPassword_DeveGerar_HashValido()
        {
            // Arrange
            var password = "senhaSegura123";
            var hasher = new BCryptPasswordHash();

            // Act
            var hash = hasher.HashPassword(password);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(hash));
            Assert.NotEqual(password, hash);
            Assert.StartsWith("$2", hash);
        }

        [Fact(DisplayName = "Verify Deve Retornar True Quando Senha Correta")]
        [Trait("Autenticação", "BCryptPasswordHashTests")]
        public void Verify_DeveRetornarTrue_QuandoSenhaCorreta()
        {
            // Arrange
            var password = "senhaSegura123";
            var hasher = new BCryptPasswordHash();
            var hash = hasher.HashPassword(password);

            // Act
            var result = hasher.Verify(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Verify Deve Retornar False Quando Senha Incorreta")]
        [Trait("Autenticação", "BCryptPasswordHashTests")]
        public void Verify_DeveRetornarFalse_QuandoSenhaIncorreta()
        {
            // Arrange
            var hasher = new BCryptPasswordHash();
            var hash = hasher.HashPassword("senhaOriginal");

            // Act
            var result = hasher.Verify("senhaErrada", hash);

            // Assert
            Assert.False(result);
        }
    }
}
