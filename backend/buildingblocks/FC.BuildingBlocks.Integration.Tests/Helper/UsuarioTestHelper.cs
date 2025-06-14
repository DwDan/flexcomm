using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FC.BuildingBlocks.Integration.Tests.DTO;
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

        public async Task<UsuarioDto> RealizarAutenticacaoAsync()
        {
            var usuario = _fakeUsuarioGenerator.Gerar();

            usuario.Id = await CriarUsuarioTeste(usuario);

            await RealizarLoginAsync(usuario);

            return usuario;
        }

        private async Task<Guid> CriarUsuarioTeste(UsuarioDto usuario)
        {
            var response = await _client.PostAsJsonAsync("api/usuario", usuario);

            response.EnsureSuccessStatusCode();

            return await ObterUsuarioId(response);
        }

        private async Task RealizarLoginAsync(UsuarioDto request)
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
