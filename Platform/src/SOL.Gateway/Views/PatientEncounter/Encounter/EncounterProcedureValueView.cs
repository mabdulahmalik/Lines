using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.PatientEncounter.Encounter;

public class EncounterProcedureValueView : IEntityTypeConfiguration<EncounterProcedureValueView>
{
    public Guid EncounterProcedureId { get; set; }
    public Guid FieldId { get; set; }
    public string Value { get; set;  }
    
    public void Configure(EntityTypeBuilder<EncounterProcedureValueView> builder)
    {
        builder.ToView("EncounterProcedureValue", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasNoKey();

        builder.Property(x => x.FieldId).HasColumnName("ProcedureFieldId");
    }
}