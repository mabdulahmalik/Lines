using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.Facility;

public class FacilityRoomPropertyValueView : IEntityTypeConfiguration<FacilityRoomPropertyValueView>
{
    public Guid RoomId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid OptionId { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoomPropertyValueView> builder)
    {
        builder.ToView("FacilityRoomPropertyValue", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}