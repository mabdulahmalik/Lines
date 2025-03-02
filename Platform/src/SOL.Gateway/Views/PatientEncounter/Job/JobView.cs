using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Job;

public class JobView : IEntityTypeConfiguration<JobView>
{
    public Guid Id { get; set; }
    public Guid? PurposeId { get; set; }
    public Guid? LineId { get; set; }
    public Guid? MedicalRecordId { get; set; }
    public JobStatus Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? StatusChangedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? OrderingProvider { get; set; }
    public string? Contact { get; set; }
    public LocationView? Location { get; set; }
    public JobScheduleView? Schedule { get; set; }
    
    public void Configure(EntityTypeBuilder<JobView> builder)
    {
        builder.ToView("vw_Job", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);
        
        builder.OwnsOne(x => x.Location, cpb =>
        {
            cpb.Property(x => x.FacilityId).HasColumnName("FacilityId");
            cpb.Property(x => x.RoomId).HasColumnName("RoomId");
            cpb.Property(x => x.Room).HasColumnName("Room");
        });

        builder.OwnsOne(x => x.Schedule, cpb =>
        {
            cpb.Property(x => x.Date).HasColumnName("ScheduledDate");
            cpb.Property(x => x.Time).HasColumnName("ScheduledTime");
        });

        builder.HasQueryFilter(x => !x.DeletedAt.HasValue);
    }
}

public class JobScheduleView
{
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
}