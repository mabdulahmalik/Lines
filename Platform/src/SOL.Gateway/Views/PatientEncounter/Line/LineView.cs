using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Line;

public class LineView : IEntityTypeConfiguration<LineView>
{
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? MedicalRecordId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; } 
    public LineDwelling? Dwelling { get; set; }
    public DateOnly? InsertedOn { get; set; }
    public LineProcedureView? InsertedWith { get; set; }
    public DateOnly? RemovedOn { get; set; }
    public LineProcedureView? RemovedWith { get; set; }    
    public LineFollowUpView? FollowUp { get; set; }
    public int? DwellTime { get; set; }
    public LocalDate? InfectedOn { get; set; }
    public bool? ExternallyPlaced { get; set; }
    public string? ExternallyPlacedBy { get; set; }
    public LocationView? Location { get; set; }
    
    public void Configure(EntityTypeBuilder<LineView> builder)
    {
        builder.ToView("vw_Line", LinesDataStore.DbSchema.PatientEncounter);
        
        builder.OwnsOne(x => x.Location, cpb =>
        {
            cpb.Property(x => x.FacilityId).HasColumnName("FacilityId");
            cpb.Property(x => x.RoomId).HasColumnName("RoomId");
        });
        
        builder.OwnsOne(x => x.FollowUp, cpb =>
        {
            cpb.Property(x => x.Date).HasColumnName("FollowUpDate");
            cpb.Property(x => x.JobId).HasColumnName("FollowUpJobId");
            cpb.Property(x => x.PurposeId).HasColumnName("FollowUpPurposeId");
        });
        
        builder.OwnsOne(x => x.InsertedWith, cpb =>
        {
            cpb.Property(x => x.EncounterId).HasColumnName("InsertedWithEncounterId");
            cpb.Property(x => x.EncounterProcedureId).HasColumnName("InsertedWithEncounterProcedureId");
        });
        
        builder.OwnsOne(x => x.RemovedWith, cpb =>
        {
            cpb.Property(x => x.EncounterId).HasColumnName("RemovedWithEncounterId");
            cpb.Property(x => x.EncounterProcedureId).HasColumnName("RemovedWithEncounterProcedureId");
        });
    }
}