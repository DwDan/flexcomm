using System.Net.Http.Json;
using System.Text.Json;
using FC.Auth.Application.Autenticacao.Login;
using FC.Auth.WebAPI.Feature.Autenticacao.Login;
using FC.BuildingBlocks.WebAPI;

namespace FC.Auth.Integration.Tests
{
    [Collection(nameof(IntegrationControllerFixtureCollection))]
    public class LoginControllerTests
    {
        private readonly AuthIntegrationTestsFixture _fixture;

        public LoginControllerTests(AuthIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Login com propriedades vazias deve retornar falhas de validação")]
        [Trait("Autenticação", "LoginController")]
        public async void Login_PropriedadesVazias_DeveRetornarFalhaValidacao()
        {
            // Arrange
            var request = new LoginRequest();

            // Act 
            var postResponse = await _fixture.Client.PostAsJsonAsync("api/autenticacao/login", request);

            // Assert
            Assert.False(postResponse.IsSuccessStatusCode);

            var json = await postResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Equal(2, result!.Errors.Count());
            Assert.Contains(LoginErrors.EmailObrigatorio, result.Errors.Select(e => e.Detail));
            Assert.Contains(LoginErrors.SenhaObrigatoria, result.Errors.Select(e => e.Detail));
        }

        [Fact(DisplayName = "Login com credencial inválida deve retornar status não autorizado")]
        [Trait("Autenticação", "LoginController")]
        public async void Login_CredencialInvalida_DeveRetornarStatusNaoAutorizado()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "teste@teste.com.br",
                Senha = "Teste@123"
            };

            // Act 
            var postResponse = await _fixture.Client.PostAsJsonAsync("api/autenticacao/login", request);

            // Assert
            Assert.False(postResponse.IsSuccessStatusCode);

            var json = await postResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Contains(LoginErrors.CredenciaisInvalidas, result.Message);
        }

        [Fact(DisplayName = "Login com credencial válida deve retornar status sucesso")]
        [Trait("Autenticação", "LoginController")]
        public async void Login_CredencialValida_DeveRetornarStatusSucesso()
        {
            // Arrange
            await _fixture.CriarUsuarioTeste();

            var request = new LoginRequest
            {
                Email = "usuario@teste.com.br",
                Senha = "Usuario-Teste@123"
            };

            // Act 
            var postResponse = await _fixture.Client.PostAsJsonAsync("api/autenticacao/login", request);

            // Assert
            Assert.True(postResponse.IsSuccessStatusCode);
        }
    }
}
