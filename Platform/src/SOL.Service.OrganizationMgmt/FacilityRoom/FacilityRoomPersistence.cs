using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Service.OrganizationMgmt.FacilityRoom.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityRoom;

class FacilityRoomPersistence : IEntityTypeConfiguration<Domain.FacilityRoom>
{
    public void Configure(EntityTypeBuilder<Domain.FacilityRoom> builder)
    {
        builder.ToTable("FacilityRoom");
        builder.Ignore(x => x.Changes);

        builder.Property(x => x.Name);
        builder.Property(x => x.FacilityId);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);

        builder.OwnsMany(x => x.Properties, oeb => {
            oeb.WithOwner()
                .HasForeignKey("RoomId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("FacilityRoomPropertyValue");

            oeb.Property<Guid>("RoomId");
            oeb.Property(p => p.Id).HasColumnName("PropertyId");
            oeb.Property(x => x.OptionId);

            oeb.HasKey("RoomId", nameof(Property.Id), nameof(Property.OptionId));
        });

        builder.Navigation(x => x.Properties)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_properties");
    }
}
