using Microsoft.EntityFrameworkCore;
using SOL.DataAccess.Conventions;

namespace SOL.Service.OrganizationMgmt;

public class OrganizationMgmtDataStore : DbContext
{
    private static readonly string Schema = "org";

    public OrganizationMgmtDataStore(DbContextOptions<OrganizationMgmtDataStore> options) 
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