using LangAppApi.Infrastructure.Utilities;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace LangAppApi.Infrastructure.Middleware
{
    public static class SerilogRequestLoggingMiddleware
    {
        public static void ConfigureSerilogRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = Utility.EnrichFromRequest;
                opts.GetLevel = Utility.ExcludeHealthChecks;
            });
        }
    }
}