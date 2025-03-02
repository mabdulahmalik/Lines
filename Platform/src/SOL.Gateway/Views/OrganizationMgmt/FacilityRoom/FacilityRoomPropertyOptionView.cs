using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

public class FacilityRoomPropertyOptionView : IEntityTypeConfiguration<FacilityRoomPropertyOptionView>
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Text { get; set; }
    public int Order { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoomPropertyOptionView> builder)
    {
        builder.ToView("FacilityRoomPropertyOption", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasKey(x => x.Id);
    }
}