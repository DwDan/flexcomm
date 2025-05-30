using AutoMapper;
using FC.Auth.Application.Autenticacao.Login;

namespace FC.Auth.WebAPI.Feature.Autenticacao.Login
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginRequest, LoginCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha));
        }
    }
}
