using Microsoft.EntityFrameworkCore;
using SOL.DataAccess.Conventions;

namespace SOL.Service.PatientEncounter;

public class PatientEncounterDataStore : DbContext
{
    private static readonly string Schema = "enc";

    public PatientEncounterDataStore(DbContextOptions<PatientEncounterDataStore> options) 
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