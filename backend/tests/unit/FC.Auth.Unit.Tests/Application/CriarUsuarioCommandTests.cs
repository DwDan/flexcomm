using AutoMapper;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Enums;
using FC.Auth.Domain.Repositories;
using FC.Auth.Domain.Validation;
using FluentValidation;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class CriarUsuarioCommandTests
    {
        private readonly CriarUsuarioCommandHandler _handler;
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CriarUsuarioCommandTests()
        {
            _repositorio = Substitute.For<IUsuarioRepositorio>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CriarUsuarioCommandHandler(_repositorio, _mapper);
        }

        [Fact(DisplayName = "Criar usuário válido deve executar com sucesso")]
        [Trait("Categoria", "Autenticação - Criar Usuario Command Handler")]
        public async Task CriarUsuario_Valido_DeveExecutarComSucesso()
        {
            // Arrange
            var command = new CriarUsuarioCommand();
            var usuario = new Usuario("teste", "teste@teste.com", "senhaHash", PerfilUsuario.Client);

            _mapper.Map<Usuario>(command).Returns(usuario);
            _repositorio.CriarAsync(usuario, CancellationToken.None).Returns(Task.CompletedTask);
            _repositorio.UnitOfWork.Commit().Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mapper.Received(1).Map<Usuario>(command);
            await _repositorio.Received(1).CriarAsync(usuario, CancellationToken.None);
            await _repositorio.UnitOfWork.Received(1).Commit();
        }

        [Fact(DisplayName = "Criar usuário inválido deve disparar exceção")]
        [Trait("Categoria", "Autenticação - Criar Usuario Command Handler")]
        public async Task CriarUsuario_Invalido_DeveDispararExcecao()
        {
            // Arrange
            var command = new CriarUsuarioCommand();
            var usuario = new Usuario("", "", "", PerfilUsuario.Client);

            _mapper.Map<Usuario>(command).Returns(usuario);

            // Act & Assert
            var result = await Assert.ThrowsAsync<ValidationException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            Assert.Equal(6, result.Errors.Count());
            Assert.Contains(UsuarioValidation.NomeObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.EmailObrigatorio, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.SenhaObrigatoria, result.Errors.Select(c => c.ErrorMessage));

            Assert.Contains(UsuarioValidation.NomeInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.EmailInvalido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(UsuarioValidation.SenhaInvalida, result.Errors.Select(c => c.ErrorMessage));

            _mapper.Received(1).Map<Usuario>(command);
            await _repositorio.DidNotReceiveWithAnyArgs().CriarAsync(usuario, CancellationToken.None);
            await _repositorio.UnitOfWork.DidNotReceiveWithAnyArgs().Commit();
        }
    }
}
