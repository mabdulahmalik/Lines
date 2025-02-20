using Microsoft.EntityFrameworkCore;
using SOL.DataAccess.Conventions;

namespace SOL.Activities.Persistence;

public class ActivitiesContext : DbContext
{
    public static readonly string Schema = "dbo";

    public ActivitiesContext(DbContextOptions<ActivitiesContext> options) 
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);
    }    

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }
}