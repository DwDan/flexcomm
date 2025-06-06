using AutoMapper;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Security;
using MediatR;
using Microsoft.VisualStudio.Services.WebApi.Jwt;

namespace FC.Auth.Application.Autenticacao.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResult>
    {
        private readonly IUsuarioRepository _repositorio;
        private readonly IPasswordHash _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(IUsuarioRepository usuarioRepositorio,
            IMapper mapper,
            IPasswordHash passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _repositorio = usuarioRepositorio;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repositorio.ObterPorEmailAsync(request.Email, cancellationToken);

            if (user == null || !_passwordHasher.Verify(request.Senha, user.SenhaHash))
                throw new InvalidCredentialsException("Login.CredenciaisInvalidas");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginCommandResult
            {
                Token = token,
                Email = user.Email,
            };
        }
    }
}