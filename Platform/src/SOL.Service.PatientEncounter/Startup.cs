using Microsoft.EntityFrameworkCore;
using SOL.DataAccess;
using SOL.Service.PatientEncounter;
using SOL.Service.PatientEncounter.Encounter;
using SOL.Service.PatientEncounter.Job.Domain.Services;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SOL.Service.PatientEncounter.Tests")]
namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddPatientEncounterContext(this IServiceCollection services, Action<PatientEncounterOptions> setServiceOptions)
    {
        var options = new PatientEncounterOptions();
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
            .AddDataAccess<PatientEncounterDataStore>(DbActions)
            .AddTransient<EncounterPhotoConverter>()
            .AddTransient<JobManager>();

        return services;
    }
}

public class PatientEncounterOptions
{
    public string? DatabaseConnectionString { get; set; }
}