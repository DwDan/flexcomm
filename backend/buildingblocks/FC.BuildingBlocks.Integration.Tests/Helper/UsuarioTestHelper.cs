using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FC.BuildingBlocks.Integration.Tests.Mocker;

namespace FC.BuildingBlocks.Integration.Tests.Helper
{
    public class UsuarioTestHelper
    {
        private readonly HttpClient _client;
        private readonly FakeUsuarioGenerator _fakeUsuarioGenerator;

        public UsuarioTestHelper(HttpClient client)
        {
            _client = client;
            _fakeUsuarioGenerator = new FakeUsuarioGenerator();
        }

        public async Task<Guid> RealizarAutenticacaoAsync()
        {
            var usuario = _fakeUsuarioGenerator.Gerar();

            var usuarioId = await CriarUsuarioTeste(usuario);

            await RealizarLoginAsync(usuario);

            return usuarioId;
        }

        public async Task<Guid> CriarUsuarioTeste()
        {
            var usuario = new Dictionary<string, string>
            {
                { "Nome", "Usuario-Teste" },
                { "Email", "usuario@teste.com.br" },
                { "Senha", "Usuario-Teste@123" }
            };

            return await CriarUsuarioTeste(usuario);
        }

        private async Task<Guid> CriarUsuarioTeste(Dictionary<string, string> request)
        {
            var response = await _client.PostAsJsonAsync("api/usuario", request);

            response.EnsureSuccessStatusCode();

            return await ObterUsuarioId(response);
        }

        private async Task RealizarLoginAsync(Dictionary<string, string> request)
        {
            var response = await _client.PostAsJsonAsync("api/autenticacao/login", request);

            response.EnsureSuccessStatusCode();

            var token = await ObterUsuarioToken(response);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<Guid> ObterUsuarioId(HttpResponseMessage response)
        {
            using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var id = json.RootElement.GetProperty("data").GetString();

            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("Usuário ID não foi retornado.");

            return Guid.Parse(id);
        }

        private async Task<string> ObterUsuarioToken(HttpResponseMessage response)
        {
            using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var token = json.RootElement.GetProperty("data").GetProperty("token").GetString();

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Token JWT não foi retornado.");

            return token;
        }
    }
}
