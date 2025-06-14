using FC.Auth.Application.Usuarios.ConfirmarEmail;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain.Security;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class ConfirmarEmailUsuarioCommandTests
    {
        private readonly ConfirmarEmailUsuarioCommandHandler _handler;
        private readonly IUsuarioRepository _repositorio;
        private readonly IEmailConfirmationTokenValidator _tokenValidator;

        public ConfirmarEmailUsuarioCommandTests()
        {
            _repositorio = Substitute.For<IUsuarioRepository>();
            _tokenValidator = Substitute.For<IEmailConfirmationTokenValidator>();
            _handler = new ConfirmarEmailUsuarioCommandHandler(_repositorio, _tokenValidator);
        }

        [Fact(DisplayName = "Confirmar email de usuário com token válido deve executar com sucesso")]
        [Trait("Autenticação", "ConfirmarEmailUsuarioCommandHandler")]
        public async Task ConfirmarEmailUsuario_TokenValido_DeveExecutarComSucesso()
        {
            // Arrange
            var usuario = new Usuario("teste", "teste@teste.com");
            var command = new ConfirmarEmailUsuarioCommand("valid-token");

            _tokenValidator.Validate(command.Token).Returns(usuario.Id);
            _repositorio.ObterPorIdAsync(usuario.Id, CancellationToken.None).Returns(usuario);
            _repositorio.UnitOfWork.CommitAsync(CancellationToken.None).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _repositorio.Received(1).Alterar(Arg.Any<Usuario>());
            await _repositorio.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
        }
    

        [Fact(DisplayName = "Confirmar email de usuário com token invalido deve executar falhas")]
        [Trait("Autenticação", "ConfirmarEmailUsuarioCommandHandler")]
        public async Task ConfirmarEmailUsuario_TokenInvalido_DeveExecutarComFalhas()
        {
            // Arrange
            var command = new ConfirmarEmailUsuarioCommand("valid-token");

            _tokenValidator.Validate(command.Token).Returns(Guid.Empty);

            // Act & Assert
            var result = await Assert.ThrowsAsync<NotFoundException>(async () => 
            {
                await _handler.Handle(command, CancellationToken.None);
            });

            Assert.NotNull(result);
            Assert.Equal("ConfirmacaoEmail.UsuarioNaoEncontrado", result.Message);
            _repositorio.DidNotReceiveWithAnyArgs().Alterar(Arg.Any<Usuario>());
            await _repositorio.UnitOfWork.DidNotReceiveWithAnyArgs().CommitAsync(CancellationToken.None);
        }
    }
}