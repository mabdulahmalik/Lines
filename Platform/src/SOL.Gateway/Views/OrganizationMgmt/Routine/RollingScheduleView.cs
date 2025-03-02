using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.OrganizationMgmt.Routine;

public record RollingScheduleView : IEntityTypeConfiguration<RollingScheduleView>
{
    public Guid RoutineId { get; set; }
    public int IntervalValue { get; set; }
    public DurationUnit IntervalUnit { get; set; }
    public int DelayValue { get; set; }
    public DurationUnit DelayUnit { get; set; }
    
    public void Configure(EntityTypeBuilder<RollingScheduleView> builder)
    {
        builder.ToView("RoutineRolling", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}