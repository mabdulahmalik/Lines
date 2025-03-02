using Microsoft.EntityFrameworkCore;
using SOL.Gateway.Views.UserMgmt.User;

namespace SOL.Gateway;

public class LinesDataStore : DbContext
{
    public DbSet<UserProfileExt> UserProfileExts => Set<UserProfileExt>();
    
    public LinesDataStore(DbContextOptions<LinesDataStore> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public class DbSchema
    {
        public static readonly string OrganizationMgmt = "org";
        public static readonly string PatientEncounter = "enc";
        public static readonly string UserMgmt = "usr";
    }
}