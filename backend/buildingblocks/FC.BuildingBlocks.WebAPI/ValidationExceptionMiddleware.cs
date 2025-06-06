using System.Net;
using System.Text.Json;
using FC.BuildingBlocks.Core.Exception;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Services.WebApi.Jwt;

namespace FC.BuildingBlocks.WebAPI
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Func<string, string> _localize;

        public ValidationExceptionMiddleware(RequestDelegate next, Func<string, string>? localize = null)
        {
            _next = next;
            _localize = localize ?? (code => code); 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ApiResponse { Success = false };

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = _localize("Error.ValidationFailed");
                    response.Errors = validationEx.Errors.Select(error => new ValidationErrorDetail
                    {
                        Error = error.ErrorCode,
                        Detail = _localize(error.ErrorCode)
                    });
                    break;

                case InvalidCredentialsException invalidCredentialsEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = _localize(invalidCredentialsEx.Message);
                    break;

                case NotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = _localize(notFoundEx.Message);
                    break;

                case BadRequestException badRequestEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = _localize(badRequestEx.Message);
                    break;

                case PersistenceException persistenceEx:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = _localize(persistenceEx.Message);
                    break;

                case BusinessException businessEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = _localize(businessEx.Message);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = _localize("Error.GenericError"); 
                    break;
            }

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
