using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.Facility.View;

public class FacilityProcedureView: IEntityTypeConfiguration<FacilityProcedureView>
{
    public Guid FacilityId { get; set; }
    public Guid ProcedureId { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityProcedureView> builder)
    {
        builder.ToView("FacilityProcedure");
        builder.HasNoKey();
    }
}