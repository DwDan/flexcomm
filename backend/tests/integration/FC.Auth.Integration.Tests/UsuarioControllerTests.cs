using System.Net.Http.Json;
using System.Text.Json;
using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.AlterarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.CriarUsuario;
using FC.BuildingBlocks.WebAPI;

namespace FC.Auth.Integration.Tests
{
    [Collection(nameof(IntegrationControllerFixtureCollection))]
    public class UsuarioControllerTests
    {
        private readonly HttpClient _client;
        private readonly AuthIntegrationTestsFixture _fixture;

        public UsuarioControllerTests(AuthIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
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

        [Fact(DisplayName = "Alterar usuário válido deve executar com sucesso")]
        [Trait("Autenticação", "UsuarioController")]
        public async void AlterarUsuario_Valido_DeveExecutarComSucesso()
        {
            // Arrange
            var _usuarioId = await _fixture.RealizarAutenticacaoAsync();

            var request = new AlterarUsuarioRequest
            {
                Id = _usuarioId,
                Endereco = new AlterarUsuarioEnderecoRequest()
                {
                    Bairro = "Bairro Teste",
                    Cidade = "Cidade Teste",
                    Estado = "Estado Teste",
                    Numero = "123",
                    Logradouro = "Logradouro Teste",
                    Cep = "12345-678"
                },
                NomeCompleto = new AlterarUsuarioNomeCompletoRequest()
                {
                    PrimeiroNome = "Primeiro Nome Teste",
                    UltimoNome = "Ultimo Nome Teste"
                },
                Telefone = new AlterarUsuarioNumeroTelefoneRequest()
                {
                    Ddd = "11",
                    Numero = "987654321"
                }
            };

            // Act 
            var putResponse = await _client.PutAsJsonAsync($"api/usuario/{request.Id}", request);

            // Assert
            Assert.True(putResponse.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Alterar usuário inválido deve executar com falha")]
        [Trait("Autenticação", "UsuarioController")]
        public async void AlterarUsuario_Invalido_DeveExecutarComFalha()
        {
            // Arrange
            var _usuarioId = await _fixture.RealizarAutenticacaoAsync();

            var request = new AlterarUsuarioRequest
            {
                Id = _usuarioId,
                Endereco = new AlterarUsuarioEnderecoRequest(),
                NomeCompleto = new AlterarUsuarioNomeCompletoRequest(),
                Telefone = new AlterarUsuarioNumeroTelefoneRequest()
            };

            // Act 
            var putResponse = await _client.PutAsJsonAsync($"api/usuario/{request.Id}", request);

            // Assert
            Assert.False(putResponse.IsSuccessStatusCode);

            var json = await putResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Equal(15, result.Errors.Count());
            Assert.Contains(AlterarUsuarioCommandValidator.NomeObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.NomeInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.SobrenomeObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.SobrenomeInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.LogradouroObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.NumeroObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.BairroObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.CidadeObrigatoria, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.EstadoObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.CepObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.CepInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.DddObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.DddInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.NumeroTelefoneObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(AlterarUsuarioCommandValidator.NumeroTelefoneInvalido, result.Errors.Select(c => c.Detail));
        }

        [Fact(DisplayName = "Criar usuário com email duplicado deve executar com falha")]
        [Trait("Autenticação", "UsuarioController")]
        public async void CriarUsuario_ComEmailDuplicado_DeveExecutarComFalha()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "teste",
                Email = "teste2@teste.com.br",
                Senha = "Teste@123"
            };

            // Act 
            var postResponse = await _client.PostAsJsonAsync("api/usuario", request);
            var postResponse2 = await _client.PostAsJsonAsync("api/usuario", request);

            // Assert
            Assert.True(postResponse.IsSuccessStatusCode);
            Assert.False(postResponse2.IsSuccessStatusCode);
        }
    }
}
