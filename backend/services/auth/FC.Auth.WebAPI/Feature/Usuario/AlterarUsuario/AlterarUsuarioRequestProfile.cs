using AutoMapper;
using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Application.Usuarios.AlterarUsuario.DTO;

namespace FC.Auth.WebAPI.Feature.Usuario.AlterarUsuario
{
    public class AlterarUsuarioRequestProfile : Profile
    {
        public AlterarUsuarioRequestProfile()
        {
            CreateMap<AlterarUsuarioNomeCompletoRequest, AlterarUsuarioNomeCompletoDto>();
            CreateMap<AlterarUsuarioEnderecoRequest, AlterarUsuarioEnderecoDto>();
            CreateMap<AlterarUsuarioNumeroTelefoneRequest, AlterarUsuarioNumeroTelefoneDto>();
            CreateMap<AlterarUsuarioRequest, AlterarUsuarioCommand>();
        }
    }
}
