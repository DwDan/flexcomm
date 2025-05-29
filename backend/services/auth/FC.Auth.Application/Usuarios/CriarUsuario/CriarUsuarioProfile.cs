using AutoMapper;
using FC.Auth.Domain.Entities;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioProfile : Profile
    {
        public CriarUsuarioProfile()
        {
            CreateMap<CriarUsuarioCommand, Usuario>()
                .ConstructUsing(src => new Usuario(src.Nome, src.Email, src.Senha, src.Perfil));
        }
    }
}
