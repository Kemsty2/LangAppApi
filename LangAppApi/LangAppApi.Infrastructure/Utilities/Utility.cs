using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;

namespace LangAppApi.Infrastructure.Utilities
{
    public static class Utility
    {
        /// <summary>
        /// Check if an endpoint contains some keys
        /// </summary>
        /// <param name="ctx">the httpContext</param>
        /// <param name="name">the key to check</param>
        /// <returns>True or False</returns>
        private static bool CheckEndpoint(HttpContext ctx, string name = "Health checks")
        {
            var endpoint = ctx.GetEndpoint();
            if (endpoint != null) // same as !(endpoint is null)
            {
                return string.Equals(
                    endpoint.DisplayName,
                    name,
                    StringComparison.Ordinal);
            }
            // No endpoint, so not a health check endpoint
            return false;
        }

        /// <summary>
        /// Exclude HealthCheck From Request Logging
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="_"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception ex) =>
            ex != null
                ? LogEventLevel.Error
                : ctx.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : CheckEndpoint(ctx) // Not an error, check if it was a health check
                        ? LogEventLevel.Verbose // Was a health check, use Verbose
                        : LogEventLevel.Information;

        /// <summary>
        /// Enrich Request Logging
        /// </summary>
        /// <param name="diagnosticContext"></param>
        /// <param name="httpContext"></param>
        public static void EnrichFromRequest(
            IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }
    }
}