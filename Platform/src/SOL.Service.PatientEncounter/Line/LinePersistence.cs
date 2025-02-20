using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Line;

class LinePersistence : IEntityTypeConfiguration<Domain.Line>
{
    public void Configure(EntityTypeBuilder<Domain.Line> builder)
    {
        builder.ToTable("Line");

        builder.Ignore(x => x.Changes);
        builder.Ignore(x => x.Dwelling);
        builder.Ignore(x => x.FacilityRoomId);
        
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);
        builder.Property(x => x.Name).HasMaxLength(80);
        builder.Property(x => x.Type).HasMaxLength(50);
        builder.Property(x => x.InsertedOn);
        builder.Property(x => x.InsertedWith);
        builder.Property(x => x.RemovedOn);
        builder.Property(x => x.RemovedWith);
        builder.Property(x => x.FollowUpId);
        builder.Property(x => x.InfectedOn);
        builder.Property(x => x.MedicalRecordId);
        builder.Property(x => x.ExternallyPlaced);
        builder.Property(x => x.ExternallyPlacedBy).HasMaxLength(255);

        builder.OwnsMany<TimeStamped<Guid>>(x => x.EncounterIds, oeb => {
            oeb.WithOwner()
                .HasForeignKey("LineId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("LineEncounter");

            oeb.Property<Guid>("LineId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("EncounterId");

            oeb.HasKey("LineId", nameof(TimeStamped<Guid>.TimeStamp), nameof(TimeStamped<Guid>.Value));
        });

        builder.OwnsMany<TimeStamped<Guid>>("_facilityRoomIds", oeb => {
            oeb.WithOwner()
                .HasForeignKey("LineId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("LineLocation");

            oeb.Property<Guid>("LineId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("RoomId");

            oeb.HasKey("LineId", nameof(TimeStamped<Guid>.TimeStamp), nameof(TimeStamped<Guid>.Value));
        });        
        
        builder.Navigation(x => x.EncounterIds)
            .HasField("_encounterIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Navigation("_facilityRoomIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);        
    }
}