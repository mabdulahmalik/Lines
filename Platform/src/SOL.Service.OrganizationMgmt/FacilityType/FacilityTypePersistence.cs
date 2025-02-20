using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.FacilityType;

class FacilityTypePersistence : IEntityTypeConfiguration<Domain.FacilityType>
{
    public void Configure(EntityTypeBuilder<Domain.FacilityType> builder)
    {
        builder.ToTable("FacilityType");
        builder.Ignore(x => x.Changes);

        builder.Property(x => x.Name).HasMaxLength(80);
        builder.Property(x => x.Order);
        builder.Property(x => x.Active);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);        

        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Properties, oeb => {
            oeb.WithOwner()
                .HasForeignKey("FacilityTypeId")
                .HasPrincipalKey(x => x.Id);
            
            oeb.ToTable("FacilityRoomProperty");

            oeb.Property<Guid>("FacilityTypeId");
            oeb.Property(x => x.Name);
            oeb.Property(x => x.Order);
            
            oeb.HasKey(x => x.Id);

            oeb.OwnsMany(x => x.Options, bb => {
                bb.WithOwner()
                    .HasForeignKey("PropertyId")
                    .HasPrincipalKey(x => x.Id);

                bb.ToTable("FacilityRoomPropertyOption");

                bb.Property<Guid>("PropertyId");
                bb.Property(x => x.Text).HasMaxLength(255);
                bb.Property(x => x.Order);

                bb.HasKey(x => x.Id);
            });

            oeb.Navigation(x => x.Options)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_options");
        });

        builder.Navigation(x => x.Properties)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_properties");
    }
}