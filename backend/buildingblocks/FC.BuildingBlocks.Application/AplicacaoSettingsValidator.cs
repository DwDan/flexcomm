using FluentValidation;

namespace FC.BuildingBlocks.Application
{
    public class AplicacaoSettingsValidator : AbstractValidator<AplicacaoSettings>
    {
        public AplicacaoSettingsValidator()
        {
            RuleFor(x => x.UrlBase)
                .NotEmpty().WithMessage("Configuração Application.UrlBase obrigatória.");
        }
    }
}
