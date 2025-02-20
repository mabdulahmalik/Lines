using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.Facility.View;

public class FacilityPurposeView : IEntityTypeConfiguration<FacilityPurposeView>
{
    public Guid FacilityId { get; set; }
    public Guid PurposeId { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityPurposeView> builder)
    {
        builder.ToView("FacilityPurpose");
        builder.HasNoKey();
    }
}