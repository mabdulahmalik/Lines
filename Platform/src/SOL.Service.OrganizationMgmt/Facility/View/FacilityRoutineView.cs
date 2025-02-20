using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.Facility.View;

public class FacilityRoutineView : IEntityTypeConfiguration<FacilityRoutineView>
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid RoutineId { get; set; }
    public string Name { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoutineView> builder)
    {
        builder.ToView("FacilityRoutine");
        builder.HasKey(x => x.Id);
    }
}