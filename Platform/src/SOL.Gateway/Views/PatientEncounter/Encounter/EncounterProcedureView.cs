using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.PatientEncounter.Encounter;

public class EncounterProcedureView : IEntityTypeConfiguration<EncounterProcedureView>
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    public Guid ProcedureId { get; set; }
    public DateTime PerformedAt { get; set; }
    public Guid PerformedBy { get; set; }
    
    public void Configure(EntityTypeBuilder<EncounterProcedureView> builder)
    {
        builder.ToView("EncounterProcedure", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);
    }
}