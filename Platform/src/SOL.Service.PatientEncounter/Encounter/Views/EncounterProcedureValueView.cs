using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Encounter.Views;

public class EncounterProcedureValueView : IEntityTypeConfiguration<EncounterProcedureValueView>
{
    public Guid EncounterProcedureId { get; set; }
    public Guid FieldId { get; set; }
    public string Value { get; set;  }
    
    public void Configure(EntityTypeBuilder<EncounterProcedureValueView> builder)
    {
        builder.ToView("EncounterProcedureValue");
        builder.HasNoKey();

        builder.Property(x => x.FieldId).HasColumnName("ProcedureFieldId");
    }
}