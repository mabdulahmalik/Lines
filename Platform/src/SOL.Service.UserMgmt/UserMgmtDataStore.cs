using Microsoft.EntityFrameworkCore;
using SOL.DataAccess.Conventions;
using SOL.Service.UserMgmt.User;
using SOL.Service.UserMgmt.User.Domain;

namespace SOL.Service.UserMgmt;

public class UserMgmtDataStore : DbContext
{
    private static readonly string Schema = "usr";

    public DbSet<UserProfileExt> UserProfileExts => Set<UserProfileExt>();
    public DbSet<UserStatus> UserStatuses => Set<UserStatus>();

    public UserMgmtDataStore(DbContextOptions<UserMgmtDataStore> options) 
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new DomainModelConvention());
        base.ConfigureConventions(configurationBuilder);
    }
}