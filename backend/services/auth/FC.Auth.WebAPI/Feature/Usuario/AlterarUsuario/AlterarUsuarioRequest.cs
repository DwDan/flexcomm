namespace FC.Auth.WebAPI.Feature.Usuario.AlterarUsuario
{
    public class AlterarUsuarioRequest
    {
        public Guid Id { get; set; }
        public AlterarUsuarioNomeCompletoRequest NomeCompleto { get; set; }
        public AlterarUsuarioEnderecoRequest? Endereco { get; set; }
        public AlterarUsuarioNumeroTelefoneRequest? Telefone { get; set; }
    }
}
