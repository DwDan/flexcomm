using FC.Auth.WebAPI.Resources;
using FC.BuildingBlocks.WebAPI;
using Microsoft.Extensions.Localization;

namespace FC.Auth.WebAPI.Middlewares
{
    public class LocalizedValidationExceptionMiddleware : ValidationExceptionMiddleware
    {
        public LocalizedValidationExceptionMiddleware(
            RequestDelegate next,
            IStringLocalizer<Messages> localizer)
            : base(next, errorCode => localizer[errorCode])
        {
        }
    }
}
