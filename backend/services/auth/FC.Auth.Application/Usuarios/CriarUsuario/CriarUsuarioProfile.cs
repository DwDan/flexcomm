using AutoMapper;
using FC.Auth.Domain.Entities;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioProfile : Profile
    {
        public CriarUsuarioProfile()
        {
            CreateMap<CriarUsuarioCommand, Usuario>()
                .ConstructUsing(src => new Usuario(src.Nome, src.Email))
                .ForMember(dest => dest.NomeCompleto, opt => opt.Ignore())
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.Ignore())
                .ForMember(dest => dest.Endereco, opt => opt.Ignore())
                .ForMember(dest => dest.Telefone, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
