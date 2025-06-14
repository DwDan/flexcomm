using FluentValidation;

namespace FC.Messaging.EmailService.Infrastructure.Smtp
{
    public class SmtpSettingsValidator : AbstractValidator<SmtpSettings>
    {
        public SmtpSettingsValidator()
        {
            RuleFor(x => x.From)
                .NotEmpty().WithMessage("Configuração Smtp.From obrigatória.");

            RuleFor(x => x.Host)
                .NotEmpty().WithMessage("Configuração Smtp.Host obrigatória.");

            RuleFor(x => x.Port)
                .GreaterThan(0).WithMessage("Configuração Smtp.Port deve ser um número positivo válido.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Configuração Smtp.Username obrigatória.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Configuração Smtp.Password obrigatória.");
        }
    }
}
