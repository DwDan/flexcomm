using FC.Auth.WebAPI.Resources;
using FC.BuildingBlocks.WebAPI;
using Microsoft.Extensions.Localization;

namespace FC.Auth.WebAPI.Middlewares
{
    public class LocalizedValidationExceptionMiddleware : ValidationExceptionLoggedMiddleware
    {
        public LocalizedValidationExceptionMiddleware(
            RequestDelegate next,
            ILogger<ValidationExceptionLoggedMiddleware> logger,
            IStringLocalizer<Messages> localizer)
            : base(next, logger, errorCode => localizer[errorCode])
        {
        }
    }
}
