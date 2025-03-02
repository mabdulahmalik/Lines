using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.Facility;

public class FacilityPurposeView : IEntityTypeConfiguration<FacilityPurposeView>
{
    public Guid FacilityId { get; set; }
    public Guid PurposeId { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityPurposeView> builder)
    {
        builder.ToView("FacilityPurpose", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}