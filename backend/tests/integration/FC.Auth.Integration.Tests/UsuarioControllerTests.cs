using System.Net.Http.Json;
using System.Text.Json;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.CriarUsuario;
using FC.BuildingBlocks.WebAPI;

namespace FC.Auth.Integration.Tests
{
    [Collection(nameof(IntegrationControllerFixtureCollection))]
    public class UsuarioControllerTests
    {
        private readonly HttpClient _client;

        public UsuarioControllerTests(AuthIntegrationTestsFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact(DisplayName = "Criar usuário válido deve executar com sucesso")]
        [Trait("Autenticação", "UsuarioController")]
        public async void CriarUsuario_Valido_DeveExecutarComSucesso()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "teste",
                Email = "teste@teste.com.br",
                Senha = "Teste@123"
            };

            // Act 
            var postResponse = await _client.PostAsJsonAsync("api/usuario", request);

            // Assert
            Assert.True(postResponse.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Criar usuário inválido deve executar com falha")]
        [Trait("Autenticação", "UsuarioController")]
        public async void CriarUsuario_Invalido_DeveExecutarComFalha()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "",
                Email = "",
                Senha = ""
            };

            // Act 
            var postResponse = await _client.PostAsJsonAsync("api/usuario", request);

            // Assert
            Assert.False(postResponse.IsSuccessStatusCode);

            var json = await postResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Equal(10, result!.Errors.Count());
            Assert.Contains(CriarUsuarioCommandValidator.NomeObrigatorio, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.EmailObrigatorio, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaObrigatoria, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.NomeInvalido, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.EmailInvalido, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaTamanhoCaracteres, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaLetraMaiuscula, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaLetraMinuscula, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaNumero, result.Errors.Select(e => e.Detail));
            Assert.Contains(CriarUsuarioCommandValidator.SenhaCaracter, result.Errors.Select(e => e.Detail));
        }
    }
}
