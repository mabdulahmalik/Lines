using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SOL.ServiceMesh;

static class HealthCheckBuilder
{
    public static IServiceCollection AddHealthCheckCfg(this IServiceCollection services,
        ConnectionStrings connectionStrings)
    {
        var healthChecks = services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
        
        // ServiceBus
        // RedisCache
        // SQLServer
        // BlobStorage
        // Keycloak
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