using SOL.Abstractions.Persistence;
using SOL.Service.TenantMgmt.Tenant;
using SOL.Service.TenantMgmt.Tenant.Domain;
using SOL.Service.TenantMgmt.Tenant.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddTenantMgmtContext(this IServiceCollection services)
    {
        services
            .AddTransient<IAggregateRepository<Tenant>, TenantRepository>()
            .AddTransient<TenantManager>();

        return services;
    }
}