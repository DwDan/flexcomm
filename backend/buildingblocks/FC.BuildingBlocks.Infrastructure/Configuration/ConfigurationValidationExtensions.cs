using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.BuildingBlocks.Infrastructure.Configuration
{
    public static class ConfigurationValidationExtensions
    {
        public static void EnsureSettings<TSettings, TValidator>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where TValidator : AbstractValidator<TSettings>, new()
            where TSettings : class, new()
        {
            services.Configure<TSettings>(configuration.GetSection(sectionName));

            var settings = configuration.GetSection(sectionName).Get<TSettings>();
            if (settings is null)
                throw new Exception($"{sectionName} config não encontrada no appsettings.json.");

            var validator = new TValidator();
            var result = validator.Validate(settings);
            if (!result.IsValid)
                throw new Exception($"{sectionName} config inválida: " +
                    string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
