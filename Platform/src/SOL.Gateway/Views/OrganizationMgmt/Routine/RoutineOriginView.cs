using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.OrganizationMgmt.Routine;

public class RoutineOriginView : IEntityTypeConfiguration<RoutineOriginView>
{
    public Guid RoutineId { get; set; }
    public Guid ProcedureId { get; set; }
    public Guid? PropertyId { get; set; }
    public string? PropertyValue { get; set; }
    
    public void Configure(EntityTypeBuilder<RoutineOriginView> builder)
    {
        builder.ToView("RoutineOrigin", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}