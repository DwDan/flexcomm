using AutoMapper;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Enums;

namespace FC.Auth.WebAPI.Feature.Usuario.CriarUsuario
{
    public class CriarUsuarioProfile : Profile
    {
        public CriarUsuarioProfile()
        {
            CreateMap<CriarUsuarioRequest, CriarUsuarioCommand>()
                .ForMember(dst => dst.Perfil, opt => opt.MapFrom(src => PerfilUsuario.Client));
        }
    }
}
