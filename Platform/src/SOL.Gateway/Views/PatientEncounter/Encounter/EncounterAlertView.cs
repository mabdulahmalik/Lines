using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Encounter;

public class EncounterAlertView : IEntityTypeConfiguration<EncounterAlertView>
{
    public Guid EncounterId { get; set; }
    public EncounterAlertType Type { get; set; }
    public DateTime AlertedAt { get; set; }
    public Guid AlertedBy { get; set; }
    public string? Text { get; set; }
    
    public void Configure(EntityTypeBuilder<EncounterAlertView> builder)
    {
        builder.ToView("EncounterAlert", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasNoKey();
    }
}