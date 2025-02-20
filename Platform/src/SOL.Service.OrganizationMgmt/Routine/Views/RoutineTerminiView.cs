using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.OrganizationMgmt.Routine.Views;

public class RoutineTerminiView : IEntityTypeConfiguration<RoutineTerminiView>
{
    public Guid RoutineId { get; set; }
    public Guid ProcedureId { get; set; }
    public Guid? PropertyId { get; set; }
    public string? PropertyValue { get; set; }
    
    public void Configure(EntityTypeBuilder<RoutineTerminiView> builder)
    {
        builder.ToView("RoutineTerminus");
        builder.HasNoKey();
    }
}