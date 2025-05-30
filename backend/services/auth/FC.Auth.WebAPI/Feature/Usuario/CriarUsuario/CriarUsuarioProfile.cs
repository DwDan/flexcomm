using AutoMapper;
using FC.Auth.Application.Usuarios.CriarUsuario;

namespace FC.Auth.WebAPI.Feature.Usuario.CriarUsuario
{
    public class CriarUsuarioProfile : Profile
    {
        public CriarUsuarioProfile()
        {
            CreateMap<CriarUsuarioRequest, CriarUsuarioCommand>();
        }
    }
}
