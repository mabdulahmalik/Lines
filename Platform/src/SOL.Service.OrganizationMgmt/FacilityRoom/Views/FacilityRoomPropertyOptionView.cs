using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Views;

public class FacilityRoomPropertyOptionView : IEntityTypeConfiguration<FacilityRoomPropertyOptionView>
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Text { get; set; }
    public int Order { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoomPropertyOptionView> builder)
    {
        builder.ToView("FacilityRoomPropertyOption");
        builder.HasKey(x => x.Id);
    }
}