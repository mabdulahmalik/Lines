using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.Facility;

public class FacilityRoutineSpecView : IEntityTypeConfiguration<FacilityRoutineSpecView>
{
    public Guid FacilityRoutineId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid PropertyValue { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoutineSpecView> builder)
    {
        builder.ToView("FacilityRoutineSpec", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}