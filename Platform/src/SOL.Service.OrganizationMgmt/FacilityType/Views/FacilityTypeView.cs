using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.FacilityType.Views;

public class FacilityTypeView : IEntityTypeConfiguration<FacilityTypeView>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool? Active { get; set; }
    public int Order { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }   
    
    public void Configure(EntityTypeBuilder<FacilityTypeView> builder)
    {
        builder.ToView("FacilityType");
        builder.HasKey(x => x.Id);
    }
}