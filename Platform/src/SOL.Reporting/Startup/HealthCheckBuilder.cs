using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SOL.Reporting;

static class HealthCheckBuilder
{
    public static IServiceCollection AddHealthCheckCfg(this IServiceCollection services,
        string sqlServerConnectionString)
    {
        var healthChecks = services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
        
        // SQLServer
        // Configuration

        return services;
    }
    
    public static IApplicationBuilder UseHealthCheckCfg(this WebApplication app)
    {
        app.MapHealthChecks("/health/live", new()
        {
            Predicate = _ => false
        });
        
        app.MapHealthChecks("/health/ready", new()
        {
            Predicate = _ => true
        });

        return app;
    }
}