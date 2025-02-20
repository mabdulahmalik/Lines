using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Line.View;

public class LineEncounterView : IEntityTypeConfiguration<LineEncounterView>
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    
    public void Configure(EntityTypeBuilder<LineEncounterView> builder)
    {
        builder.ToView("LineEncounter");
        builder.HasNoKey();

        builder.Property(x => x.Id).HasColumnName("LineId");
    }
}