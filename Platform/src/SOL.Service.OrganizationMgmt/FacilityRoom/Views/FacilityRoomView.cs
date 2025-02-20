using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Views;

public class FacilityRoomView : IEntityTypeConfiguration<FacilityRoomView>
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoomView> builder)
    {
        builder.ToView("FacilityRoom");
        builder.HasKey(x => x.Id);
    }
}