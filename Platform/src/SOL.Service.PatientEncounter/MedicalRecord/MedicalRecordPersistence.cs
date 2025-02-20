using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Service.PatientEncounter.MedicalRecord.Domain;

namespace SOL.Service.PatientEncounter.MedicalRecord;

class MedicalRecordPersistence : IEntityTypeConfiguration<Domain.MedicalRecord>
{
    public void Configure(EntityTypeBuilder<Domain.MedicalRecord> builder)
    {
        builder.ToTable("MedicalRecord");
        builder.Ignore(x => x.Changes);

        builder.Property(x => x.FacilityId);
        builder.Property(x => x.Number).HasMaxLength(50);
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.Birthday);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);
        
        builder.OwnsMany(x => x.Observations, y =>
        {
            y.WithOwner()
                .HasForeignKey("MedicalRecordId")
                .HasPrincipalKey(x => x.Id);
            
            y.ToTable("MedicalRecordObservation");
            
            y.Property<Guid>("MedicalRecordId");
            y.Property(z => z.ObjectId);
            y.Property(z => z.Type);
            y.Property(z => z.Timestamp);

            y.HasKey("MedicalRecordId", nameof(Observation.ObjectId)
                , nameof(Observation.Type), nameof(Observation.Timestamp));
        });
    }
}