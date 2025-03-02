using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Encounter;

public class EncounterAssignmentView : IEntityTypeConfiguration<EncounterAssignmentView>
{
    public Guid EncounterId { get; set; }
    public Guid? ClinicianId { get; set; }
    public EncounterAssigneePosition Position { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? WithdrawnAt { get; set; }
    
    public void Configure(EntityTypeBuilder<EncounterAssignmentView> builder)
    {
        builder.ToView("EncounterAssignment", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasNoKey();

        builder.HasQueryFilter(x => !x.WithdrawnAt.HasValue);
    }
}