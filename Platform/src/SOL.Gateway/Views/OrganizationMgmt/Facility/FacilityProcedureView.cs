using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.Facility;

public class FacilityProcedureView: IEntityTypeConfiguration<FacilityProcedureView>
{
    public Guid FacilityId { get; set; }
    public Guid ProcedureId { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityProcedureView> builder)
    {
        builder.ToView("FacilityProcedure", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}