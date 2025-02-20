using HotChocolate.Types.NodaTime;
using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RecurrenceScheduleType : ObjectType<RecurrenceScheduleView>
{
    protected override void Configure(IObjectTypeDescriptor<RecurrenceScheduleView> descriptor)
    {
        descriptor
            .Name("RecurrenceSchedule")
            .Description("A Recurrence Schedule entry.");

        descriptor
            .Field(x => x.Time)
            .Name("time")
            .Description("The time of day for Recurrence Schedule.");

        descriptor
            .Field(x => x.Days)
            .Type<ListType<IsoDayOfWeekType>>()
            .Name("days")
            .Description("The Days of Recurrence Schedule.");
    }
}