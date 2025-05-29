namespace FC.Auth.WebAPI.Feature.Usuario.CriarUsuario
{
    public class CriarUsuarioRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
