using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Job;

class JobPersistence : IEntityTypeConfiguration<Domain.Job>
{
    public void Configure(EntityTypeBuilder<Domain.Job> builder)
    {
        builder.ToTable("Job");
        
        builder.Ignore(x => x.Changes);
        builder.Ignore(x => x.Status);
        builder.Ignore(x => x.StatusChangedAt);

        builder.Property(x => x.PurposeId);
        builder.Property(x => x.RoomId);
        builder.Property(x => x.LineId);
        builder.Property(x => x.MedicalRecordId);
        builder.Property(x => x.Contact).HasMaxLength(255);
        builder.Property(x => x.OrderingProvider).HasMaxLength(255);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.DeletedAt);
        builder.Property(x => x.ModifiedAt);
        builder.Property(x => x.ScheduledDate);
        builder.Property(x => x.ScheduledTime);

        builder.OwnsMany(x => x.Notes, oeb => {
            oeb.WithOwner()
                .HasForeignKey("JobId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("JobNote");

            oeb.Property<Guid>("JobId");
            oeb.Property(x => x.UserId).HasColumnName("AuthorId");
            oeb.Property(x => x.CreatedAt);
            oeb.Property(x => x.IsPinned);
            oeb.Property(x => x.Text);
            oeb.Property(x => x.Id);
            
            oeb.HasKey("JobId", nameof(Note.Id));
        });
        
        builder.OwnsMany(x => x.Routines, oeb => {
            oeb.WithOwner()
                .HasForeignKey("JobId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("JobRoutine");

            oeb.Property<Guid>("JobId");
            oeb.Property(x => x.EncounterProcedureId).HasColumnName("EncounterProcedureId");
            oeb.Property(x => x.RoutineAssignmentId).HasColumnName("FacilityRoutineId");
            oeb.Property(x => x.Timestamp);
            
            oeb.HasKey("JobId", nameof(ActiveRoutine.EncounterProcedureId), nameof(ActiveRoutine.RoutineAssignmentId));
        });        

        builder.OwnsMany<TimeStamped<JobStatus>>("_status", oeb => { 
            oeb.WithOwner()
                .HasForeignKey("JobId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("JobStatus");

            oeb.Property<Guid>("JobId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("Status");

            oeb.HasKey("JobId", nameof(TimeStamped<JobStatus>.TimeStamp), nameof(TimeStamped<JobStatus>.Value));
        });
        
        builder.Navigation(x => x.Notes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Navigation(x => x.Routines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);        

        builder.Navigation("_status")
            .UsePropertyAccessMode(PropertyAccessMode.Field);            
    }
}

class ScheduledJobPersistence : IEntityTypeConfiguration<Domain.ScheduledJob>
{
    public void Configure(EntityTypeBuilder<Domain.ScheduledJob> builder)
    {
        builder.ToView("vw_Job");
        builder.HasKey(x => x.Id);
        
        builder.HasQueryFilter(x => !x.DeletedAt.HasValue);
    }
}