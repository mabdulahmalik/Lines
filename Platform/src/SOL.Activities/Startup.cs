using SOL.Activities.Persistence;
using Microsoft.EntityFrameworkCore;
using SOL.DataAccess;
using SOL.Activities;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddActivityTracking(this IServiceCollection services, Action<ActivityTrackingOptions> setServiceOptions)
    {
        var options = new ActivityTrackingOptions();
        setServiceOptions(options);

        void DbActions(DbContextOptionsBuilder dbOpts)
        {
            dbOpts.UseSqlServer(options.DatabaseConnectionString, sqlOpts =>
            {
                sqlOpts.UseNodaTime();
                sqlOpts.UseAzureSqlDefaults();
                sqlOpts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }

        services
            .AddDataAccess<ActivitiesContext>(DbActions);

        services.AddTransient<ActivitiesManager>();

        return services;
    }
}

public class ActivityTrackingOptions
{
    public string? DatabaseConnectionString { get; set; }
}