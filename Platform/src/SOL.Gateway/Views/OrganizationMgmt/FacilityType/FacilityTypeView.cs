using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.FacilityType;

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
        builder.ToView("FacilityType", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasKey(x => x.Id);
    }
}