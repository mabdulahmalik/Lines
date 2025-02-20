using SOL.Gateway.Schema.Common;
using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RollingScheduleDurationType : ObjectType<RollingScheduleDurationView>
{
    protected override void Configure(IObjectTypeDescriptor<RollingScheduleDurationView> descriptor)
    {
        descriptor
            .Name("RollingScheduleDuration")
            .Description("The Duration of the Rolling Interval Schedule.");

        descriptor
            .Field(t => t.Value)
            .Name("value")
            .Description("The value for the Rolling Schedule.");

        descriptor
            .Field(t => t.Unit)
            .Type<DurationUnitType>()
            .Name("unit")
            .Description("The unit for the Rolling Schedule.");
    }
}