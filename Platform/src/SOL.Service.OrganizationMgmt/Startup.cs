using Microsoft.EntityFrameworkCore;
using SOL.DataAccess;
using SOL.Service.OrganizationMgmt;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddOrganizationMgmtContext(this IServiceCollection services, Action<OrganizationMgmtOptions> setServiceOptions)
    {
        var options = new OrganizationMgmtOptions();
        setServiceOptions(options);

        void DbActions(DbContextOptionsBuilder dbOpts)
        {
            dbOpts.UseSqlServer(options.DatabaseConnectionString, sqlOpts => {
                sqlOpts.UseNodaTime();
                sqlOpts.UseAzureSqlDefaults();
                sqlOpts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }

        services
            .AddDataAccess<OrganizationMgmtDataStore>(DbActions);

        return services;
    }
}

public class OrganizationMgmtOptions
{
    public string? DatabaseConnectionString { get; set; }
}