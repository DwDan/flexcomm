using AutoMapper;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.Auth.Domain.Validation;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain;
using FluentValidation;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class CriarUsuarioCommandTests
    {
        private readonly CriarUsuarioCommandHandler _handler;
        private readonly IUsuarioRepository _repositorio;
        private readonly IMapper _mapper;
        private readonly IPasswordHash _passwordHash;

        public CriarUsuarioCommandTests()
        {
            _repositorio = Substitute.For<IUsuarioRepository>();
            _mapper = Substitute.For<IMapper>();
            _passwordHash = Substitute.For<IPasswordHash>();
            _handler = new CriarUsuarioCommandHandler(_repositorio, _mapper, _passwordHash);
        }

        [Fact(DisplayName = "Criar usuário válido deve executar com sucesso")]
        [Trait("Autenticação", "CriarUsuarioCommand")]
        public async Task CriarUsuario_Valido_DeveExecutarComSucesso()
        {
            // Arrange
            var senhaSimples = "senhaSimples";
            var senhaCriptografada = "$2senhaCriptografada";
            var command = new CriarUsuarioCommand() { Senha = senhaSimples };
            var usuario = new Usuario("teste", "teste@teste.com");

            _mapper.Map<Usuario>(command).Returns(usuario);
            _repositorio.UnitOfWork.CommitAsync(CancellationToken.None).Returns(true);
            _passwordHash.HashPassword(senhaSimples).Returns(senhaCriptografada);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _mapper.Received(1).Map<Usuario>(command);
            _repositorio.Received(1).Criar(usuario);
            _passwordHash.Received(1).HashPassword(senhaSimples);
            await _repositorio.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
        }

        [Fact(DisplayName = "Criar usuário inválido deve disparar exceção")]
        [Trait("Autenticação", "CriarUsuarioCommand")]
        public async Task CriarUsuario_Invalido_DeveDispararExcecao()
        {
            // Arrange
            var command = new CriarUsuarioCommand();
            var usuario = new Usuario("", "");

            _mapper.Map<Usuario>(command).Returns(usuario);

            // Act & Assert
            var result = await Assert.ThrowsAsync<ValidationException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            Assert.Equal(6, result.Errors.Count());
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_NomeObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_EmailObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_SenhaObrigatoria, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_NomeInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_EmailInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_SenhaInvalida, result.Errors.Select(c => c.ErrorCode));

            _mapper.Received(1).Map<Usuario>(command);
            _repositorio.DidNotReceiveWithAnyArgs().Criar(usuario);
            await _repositorio.UnitOfWork.DidNotReceiveWithAnyArgs().CommitAsync(CancellationToken.None);
        }

        [Fact(DisplayName = "Criar usuário com email duplicado deve disparar exceção")]
        [Trait("Autenticação", "CriarUsuarioCommand")]
        public async Task CriarUsuario_ComEmailDuplicado_DeveDispararExcecao()
        {
            // Arrange
            var email = "teste2@teste.com";
            var command = new CriarUsuarioCommand() { Email = email,  Senha = "senhaSimples" };
            var usuario = new Usuario("teste", email);
            _repositorio.ObterPorEmailAsync(email, CancellationToken.None).Returns(usuario);

            // Act & Assert
            var result = await Assert.ThrowsAsync<BusinessException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            Assert.Contains("teste2@teste.com", result.Message);
        }
    }
}
