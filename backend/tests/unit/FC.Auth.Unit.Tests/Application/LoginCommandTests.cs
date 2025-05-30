using AutoMapper;
using FC.Auth.Application.Autenticacao.Login;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Security;
using Microsoft.VisualStudio.Services.WebApi.Jwt;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class LoginCommandTests
    {
        private readonly LoginCommandHandler _handler;
        private readonly IUsuarioRepository _repositorio;
        private readonly IMapper _mapper;
        private readonly IPasswordHash _passwordHash;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandTests()
        {
            _repositorio = Substitute.For<IUsuarioRepository>();
            _mapper = Substitute.For<IMapper>();
            _passwordHash = Substitute.For<IPasswordHash>();
            _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>(); 
            _handler = new LoginCommandHandler(_repositorio, _mapper, _passwordHash, _jwtTokenGenerator);
        }

        [Fact(DisplayName = "Login válido deve executar com sucesso")]
        [Trait("Autenticação", "LoginCommand")]
        public async Task Login_Valido_DeveExecutarComSucesso()
        {
            // Arrange
            var email = "Usuario@Teste.com";
            var senha = "Senha@123";
            var command = new LoginCommand() { Email = email, Senha = senha };
            var usuario = new Usuario("Usuário Teste", email);
            usuario.DefinirSenhaCriptografada(senha);

            _repositorio.ObterPorEmailAsync(email, CancellationToken.None).Returns(usuario);
            _passwordHash.Verify(senha, usuario.SenhaHash).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _repositorio.Received(1).ObterPorEmailAsync(email, CancellationToken.None);
            _passwordHash.Received(1).Verify(senha, usuario.SenhaHash);
        }

        [Fact(DisplayName = "Login com senha inválida deve disparar exceção")]
        [Trait("Autenticação", "LoginCommand")]
        public async Task Login_SenhaInvalida_DeveDispararExcecao()
        {
            // Arrange
            var email = "Usuario@Teste.com";
            var senha = "Senha@123";
            var command = new LoginCommand() { Email = email, Senha = senha };
            var usuario = new Usuario("Usuário Teste", email);
            usuario.DefinirSenhaCriptografada(senha);

            _repositorio.ObterPorEmailAsync(email, CancellationToken.None).Returns(usuario);
            _passwordHash.Verify(senha, usuario.SenhaHash).Returns(false);

            // Act & Assert
            var result = await Assert.ThrowsAsync<InvalidCredentialsException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            await _repositorio.Received(1).ObterPorEmailAsync(email, CancellationToken.None);
            _passwordHash.Received(1).Verify(senha, usuario.SenhaHash);
        }

        [Fact(DisplayName = "Login com usuário inválido deve disparar exceção")]
        [Trait("Autenticação", "LoginCommand")]
        public async Task Login_UsuarioInvalido_DeveDispararExcecao()
        {
            // Arrange
            var email = "Usuario@Teste.com";
            var senha = "Senha@123";
            var command = new LoginCommand() { Email = email, Senha = senha };
            var usuario = default(Usuario);

            _repositorio.ObterPorEmailAsync(email, CancellationToken.None).Returns(usuario);

            // Act & Assert
            var result = await Assert.ThrowsAsync<InvalidCredentialsException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            await _repositorio.Received(1).ObterPorEmailAsync(email, CancellationToken.None);
            _passwordHash.DidNotReceiveWithAnyArgs().Verify(default, default);
        }
    }
}
