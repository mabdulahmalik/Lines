using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;

namespace SOL.Gateway.Views.OrganizationMgmt.Routine;

public class RecurrenceScheduleView : IEntityTypeConfiguration<RecurrenceScheduleView>
{
    public Guid RoutineId { get; set; }
    public List<IsoDayOfWeek> Days { get; set; }
    public LocalTime Time { get; set; }

    public void Configure(EntityTypeBuilder<RecurrenceScheduleView> builder)
    {
        builder.ToView("RoutineRecurrence", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasNoKey();
    }
}