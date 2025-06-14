using System.Diagnostics;
using FC.BuildingBlocks.Domain.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace FC.BuildingBlocks.WebAPI
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, 
            ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var headerValue)
                ? headerValue.ToString()
                : Guid.NewGuid().ToString();

            var correlationContext = context.RequestServices.GetRequiredService<ICorrelationContext>();
            correlationContext.Set(correlationId);

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (var activity = Activity.Current ?? new Activity("HttpRequest"))
            {
                activity.SetIdFormat(ActivityIdFormat.W3C);
                activity.AddTag("CorrelationId", correlationId);
                activity.Start();

                if (!context.Response.Headers.ContainsKey(HeaderName))
                    context.Response.Headers[HeaderName] = correlationId;

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro durante requisição com CorrelationId {CorrelationId}", correlationId);
                    throw;
                }
                finally
                {
                    activity.Stop();
                }
            }
        }
    }
}