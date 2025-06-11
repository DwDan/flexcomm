using FC.BuildingBlocks.Application;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.Application.Tests
{
    public class ValidationBehaviorTests
    {
        public record TestCommand(string Nome) : IRequest<string>;

        [Fact(DisplayName = "Deve passar se não houver validators")]
        public async Task SemValidators_DeveExecutarHandler()
        {
            // Arrange
            var behavior = new ValidationBehavior<TestCommand, string>(Enumerable.Empty<IValidator<TestCommand>>());
            var command = new TestCommand("Daniel");

            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next().Returns("ok");

            // Act
            var result = await behavior.Handle(command, next, CancellationToken.None);

            // Assert
            Assert.Equal("ok", result);
        }

        [Fact(DisplayName = "Deve lançar ValidationException se houver falhas")]
        public async Task ComErroDeValidacao_DeveLancarExcecao()
        {
            // Arrange
            var validator = Substitute.For<IValidator<TestCommand>>();
            validator.ValidateAsync(Arg.Any<ValidationContext<TestCommand>>(), Arg.Any<CancellationToken>())
                     .Returns(new ValidationResult(new[]
                     {
                         new ValidationFailure("Nome", "Nome é obrigatório.")
                     }));

            var behavior = new ValidationBehavior<TestCommand, string>(new[] { validator });
            var command = new TestCommand("");

            var next = Substitute.For<RequestHandlerDelegate<string>>();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                behavior.Handle(command, next, CancellationToken.None));

            Assert.Contains("Nome é obrigatório", ex.Message);
            await next.DidNotReceive().Invoke();
        }

        [Fact(DisplayName = "Deve chamar handler se validação for bem-sucedida")]
        public async Task ComValidacaoOk_DeveExecutarHandler()
        {
            // Arrange
            var validator = Substitute.For<IValidator<TestCommand>>();
            validator.ValidateAsync(Arg.Any<ValidationContext<TestCommand>>(), Arg.Any<CancellationToken>())
                     .Returns(new ValidationResult());

            var behavior = new ValidationBehavior<TestCommand, string>(new[] { validator });
            var command = new TestCommand("Daniel");

            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next().Returns("sucesso");

            // Act
            var result = await behavior.Handle(command, next, CancellationToken.None);

            // Assert
            Assert.Equal("sucesso", result);
            await next.Received(1).Invoke();
        }
    }
}
