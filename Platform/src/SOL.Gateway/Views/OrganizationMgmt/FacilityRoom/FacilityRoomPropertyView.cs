using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

public class FacilityRoomPropertyView : IEntityTypeConfiguration<FacilityRoomPropertyView>
{
    public Guid Id { get; set; }
    public Guid FacilityTypeId { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoomPropertyView> builder)
    {
        builder.ToView("FacilityRoomProperty", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasKey(x => x.Id);
    }
}
