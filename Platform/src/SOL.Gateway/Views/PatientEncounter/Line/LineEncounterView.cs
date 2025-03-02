using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.PatientEncounter.Line;

public class LineEncounterView : IEntityTypeConfiguration<LineEncounterView>
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    
    public void Configure(EntityTypeBuilder<LineEncounterView> builder)
    {
        builder.ToView("LineEncounter", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasNoKey();

        builder.Property(x => x.Id).HasColumnName("LineId");
    }
}