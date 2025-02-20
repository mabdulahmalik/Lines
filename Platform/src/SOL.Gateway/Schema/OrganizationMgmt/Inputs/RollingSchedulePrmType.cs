using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RollingSchedulePrmType : InputObjectType<RollingSchedule>
{
    protected override void Configure(IInputObjectTypeDescriptor<RollingSchedule> descriptor)
    {
        descriptor
            .Name("RollingSchedulePrm")
            .Description("The Parameters for the 'Rolling Interval' style Schedule in which the Routine will be executed.");

        descriptor
            .Field(x => x.Interval)
            .Type<RollingScheduleDurationPrmType>()
            .Name("interval")
            .Description("The Interval in which the Routine should repeat.");

        descriptor
            .Field(x => x.Delay)
            .Type<RollingScheduleDurationPrmType>()
            .Name("delay")
            .Description("The amount of time to Delay the start of the Routine.");
    }
}