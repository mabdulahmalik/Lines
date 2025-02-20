using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Purpose;

public class PurposePersistence : IEntityTypeConfiguration<Domain.Purpose>
{
    public void Configure(EntityTypeBuilder<Domain.Purpose> builder)
    {
        builder.ToTable("Purpose");
        builder.Ignore(x => x.Changes);
        
        builder.Property(x => x.Name).HasMaxLength(80);
        builder.Property(x => x.Order);
        builder.Property(x => x.Archived);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);
    }
}