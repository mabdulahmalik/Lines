using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter;

class EncounterPersistence : IEntityTypeConfiguration<Domain.Encounter>
{
    public void Configure(EntityTypeBuilder<Domain.Encounter> builder)
    {
        builder.ToTable("Encounter");
        
        builder.Ignore(x => x.Changes);
        builder.Ignore(x => x.Priority);
        builder.Ignore(x => x.Stage);

        builder.Property(x => x.JobId);
        builder.Property(x => x.PurposeId);
        builder.Property(x => x.FacilityRoomId).HasColumnName("RoomId");
        builder.Property(x => x.MedicalRecordId);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);

        builder.OwnsMany(x => x.Assignments, oeb => {
            oeb.WithOwner()
                .HasForeignKey("EncounterId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("EncounterAssignment");

            oeb.Property<Guid>("EncounterId");
            oeb.Property(x => x.ClinicianId);
            oeb.Property(x => x.AssignedAt);
            oeb.Property(x => x.WithdrawnAt);
            oeb.Property(x => x.Position);

            oeb.HasKey("EncounterId", "ClinicianId", "AssignedAt");
        });

        builder.OwnsMany(x => x.Procedures, oeb => {
            oeb.WithOwner()
                .HasForeignKey("EncounterId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("EncounterProcedure");

            oeb.Property<Guid>("EncounterId");
            oeb.Property(x => x.ProcedureId);
            oeb.Property(x => x.PerformedBy);
            oeb.Property(x => x.PerformedAt);

            oeb.OwnsMany(x => x.Values, bb => {
                bb.WithOwner()
                    .HasForeignKey("EncounterProcedureId")
                    .HasPrincipalKey(x => x.Id);

                bb.ToTable("EncounterProcedureValue");
                
                bb.Property<Guid>("EncounterProcedureId");
                bb.Property(x => x.FieldId).HasColumnName("ProcedureFieldId");
                bb.Property(x => x.Value).HasMaxLength(255);

                bb.HasKey("EncounterProcedureId", nameof(ProcedureValue.FieldId));
            });
            
            oeb.HasKey(nameof(Domain.Procedure.Id), "EncounterId");

            oeb.Navigation(x => x.Values)
                .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction)
                .HasField("_values");
        });

        builder.OwnsMany(x => x.Photos, oeb => {
            oeb.WithOwner()
                .HasForeignKey("EncounterId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("EncounterPhoto");

            oeb.Property<Guid>("EncounterId");
            oeb.Property(x => x.CreatedAt);
            oeb.Property(x => x.FileName).HasMaxLength(80);
            oeb.Property(x => x.ContentType).HasMaxLength(20);
            oeb.Property(x => x.Length);
        });
        
        builder.OwnsMany(x => x.Alerts, oeb => {
            oeb.WithOwner()
                .HasForeignKey("EncounterId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("EncounterAlert");

            oeb.Property<Guid>("EncounterId");
            oeb.Property(x => x.AlertedAt);
            oeb.Property(x => x.AlertedBy);
            oeb.Property(x => x.Text);
            oeb.Property(x => x.Type);
            
            oeb.HasKey("EncounterId", nameof(Alert.Type));
        });        
        
        builder.OwnsMany<TimeStamped<EncounterStage>>("_stage", oeb => {
            oeb.WithOwner()
                .HasForeignKey("EncounterId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("EncounterStage");

            oeb.Property<Guid>("EncounterId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("Stage");

            oeb.HasKey("EncounterId", nameof(TimeStamped<EncounterStage>.TimeStamp), nameof(TimeStamped<EncounterStage>.Value));
        });

        builder.OwnsMany<TimeStamped<EncounterPriority>>("_priority", oeb => {
            oeb.WithOwner()
                .HasForeignKey("EncounterId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("EncounterPriority");

            oeb.Property<Guid>("EncounterId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("Priority");

            oeb.HasKey("EncounterId", nameof(TimeStamped<EncounterPriority>.TimeStamp), nameof(TimeStamped<EncounterPriority>.Value));
        });

        builder.Navigation(x => x.Assignments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.Procedures)
            .UsePropertyAccessMode(PropertyAccessMode.Field);            

        builder.Navigation(x => x.Photos)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Navigation(x => x.Alerts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);        

        builder.Navigation("_stage")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Navigation("_priority")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}