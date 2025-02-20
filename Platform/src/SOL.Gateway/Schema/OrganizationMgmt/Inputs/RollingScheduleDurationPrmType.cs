using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RollingScheduleDurationPrmType : InputObjectType<RollingScheduleDuration>
{
    protected override void Configure(IInputObjectTypeDescriptor<RollingScheduleDuration> descriptor)
    {
        descriptor
            .Name("RollingScheduleDurationPrm")
            .Description("The Parameters for the Interval duration for a Routine.");

        descriptor
            .Field(x => x.Value)
            .Name("value")
            .Description("The value of the rolling duration.");

        descriptor
            .Field(x => x.Unit)
            .Type<DurationUnitType>()
            .Name("unit")
            .Description("The unit of the rolling duration.");
    }
}