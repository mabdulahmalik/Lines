using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Encounter;

public class EncounterView : IEntityTypeConfiguration<EncounterView>
{
    public Guid Id { get; set; }
    public Guid? JobId { get; set; }
    public Guid? PurposeId { get; set; }
    public Guid? MedicalRecordId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public EncounterStage Stage { get; set; }
    public EncounterPriority Priority { get; set; }
    public LocationView? Location { get; set; }
    
    public void Configure(EntityTypeBuilder<EncounterView> builder)
    {
        builder.ToView("vw_Encounter", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Location, cpb =>
        {
            cpb.Property(x => x.FacilityId).HasColumnName("FacilityId");
            cpb.Property(x => x.RoomId).HasColumnName("RoomId");
        });        

        builder.HasQueryFilter(x => !x.DeletedAt.HasValue);
    }
}