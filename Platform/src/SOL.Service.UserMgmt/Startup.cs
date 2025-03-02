using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt;
using SOL.Service.UserMgmt.Invitation.Domain.Services;
using SOL.Service.UserMgmt.User;
using SOL.Service.UserMgmt.User.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddUserMgmtContext(this IServiceCollection services, Action<UserMgmtOptions> setServiceOptions)
    {
        var options = new UserMgmtOptions();
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
            .AddDbContextFactory<UserMgmtDataStore>(DbActions)
            .AddDbContext<UserMgmtDataStore>(DbActions)
            .AddTransient<InvitationManager>()
            .AddTransient<IAggregateRepository<User>, UserRepository>();

        return services;
    }
}

public class UserMgmtOptions
{
    public string? DatabaseConnectionString { get; set; }
}