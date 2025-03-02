using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.Facility;

public class FacilityRoutineView : IEntityTypeConfiguration<FacilityRoutineView>
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid RoutineId { get; set; }
    public string Name { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityRoutineView> builder)
    {
        builder.ToView("FacilityRoutine", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasKey(x => x.Id);
    }
}