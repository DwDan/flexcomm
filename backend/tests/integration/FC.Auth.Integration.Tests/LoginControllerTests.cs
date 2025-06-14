using System.Net.Http.Json;
using System.Text.Json;
using FC.Auth.Integration.Tests.FixtureCollection;
using FC.Auth.WebAPI.Feature.Autenticacao.Login;
using FC.Auth.WebAPI.Resources;
using FC.BuildingBlocks.WebAPI;

namespace FC.Auth.Integration.Tests
{
    [Collection(nameof(IntegrationControllerFixtureCollection))]
    public class LoginControllerTests
    {
        private readonly AuthIntegrationTestsFixture _fixture;
        private readonly HttpClient _client;

        public LoginControllerTests(AuthIntegrationTestsFixture fixture)
        {
            _fixture = fixture; 
            _client = fixture.Clients["Auth"];
        }

        [Fact(DisplayName = "Login com propriedades vazias deve retornar falhas de validação")]
        [Trait("Autenticação", "LoginController")]
        public async void Login_PropriedadesVazias_DeveRetornarFalhaValidacao()
        {
            // Arrange
            var request = new LoginRequest();

            // Act 
            var postResponse = await _client.PostAsJsonAsync("api/autenticacao/login", request);

            // Assert
            Assert.False(postResponse.IsSuccessStatusCode);

            var json = await postResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Equal(2, result!.Errors.Count());
            Assert.Contains(Messages.Login_EmailObrigatorio, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.Login_SenhaObrigatoria, result.Errors.Select(e => e.Detail));
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
            var postResponse = await _client.PostAsJsonAsync("api/autenticacao/login", request);

            // Assert
            Assert.False(postResponse.IsSuccessStatusCode);

            var json = await postResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Contains(Messages.Login_CredenciaisInvalidas, result.Message);
        }

        [Fact(DisplayName = "Login com credencial válida deve retornar status sucesso")]
        [Trait("Autenticação", "LoginController")]
        public async void Login_CredencialValida_DeveRetornarStatusSucesso()
        {
            // Arrange & Act 
            var usuario = await _fixture.UsuarioTestHelper.RealizarAutenticacaoAsync();

            // Assert
            Assert.NotEqual(Guid.Empty, usuario.Id);
        }
    }
}
