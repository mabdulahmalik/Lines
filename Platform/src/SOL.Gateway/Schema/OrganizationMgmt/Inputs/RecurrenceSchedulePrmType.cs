using HotChocolate.Types.NodaTime;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RecurrenceSchedulePrmType : InputObjectType<RecurrenceSchedule>
{
    protected override void Configure(IInputObjectTypeDescriptor<RecurrenceSchedule> descriptor)
    {
        descriptor
            .Name("RecurrenceSchedulePrm")
            .Description("The Parameters for the 'Recurrence' style Schedule in which the Routine will be executed.");

        descriptor
            .Field(x => x.Time)
            .Name("time")
            .Description("The time for which the Routine will reoccur.");

        descriptor
            .Field(x => x.Days)
            .Type<ListType<IsoDayOfWeekType>>()
            .Name("days")
            .Description("The days for which the Routine will reoccur..");
    }
}