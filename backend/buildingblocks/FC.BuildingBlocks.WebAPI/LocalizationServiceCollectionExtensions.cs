using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace FC.BuildingBlocks.WebAPI
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            var supportedCultures = new[] { "pt-BR", "en-US" };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
                SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = localizationOptions.DefaultRequestCulture;
                options.SupportedCultures = localizationOptions.SupportedCultures;
                options.SupportedUICultures = localizationOptions.SupportedUICultures;
            });

            return services;
        }
    }
}