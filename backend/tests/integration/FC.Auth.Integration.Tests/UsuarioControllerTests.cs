using System.Net.Http.Json;
using System.Text.Json;
using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.AlterarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.CriarUsuario;
using FC.Auth.WebAPI.Resources;
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
            Assert.Contains(Messages.CriarUsuario_NomeObrigatorio, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_EmailObrigatorio, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_SenhaObrigatoria, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_NomeInvalido, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_EmailInvalido, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_SenhaTamanhoCaracteres, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_SenhaLetraMaiuscula, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_SenhaLetraMinuscula, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_SenhaNumero, result.Errors.Select(e => e.Detail));
            Assert.Contains(Messages.CriarUsuario_SenhaCaracter, result.Errors.Select(e => e.Detail));
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
            Assert.Contains(Messages.AlterarUsuario_NomeObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_NomeInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_SobrenomeObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_SobrenomeInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_LogradouroObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_NumeroObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_BairroObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_CidadeObrigatoria, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_EstadoObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_CepObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_CepInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_DddObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_DddInvalido, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_NumeroTelefoneObrigatorio, result.Errors.Select(c => c.Detail));
            Assert.Contains(Messages.AlterarUsuario_NumeroTelefoneInvalido, result.Errors.Select(c => c.Detail));
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
