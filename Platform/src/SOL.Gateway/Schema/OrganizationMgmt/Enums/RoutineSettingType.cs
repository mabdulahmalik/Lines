using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineSettingType : EnumType<RoutineSetting>
{
    protected override void Configure(IEnumTypeDescriptor<RoutineSetting> descriptor)
    {
        descriptor
            .Name("RoutineSetting")
            .Description("The settings for a Routine.");

        descriptor
            .Value(RoutineSetting.FollowUp)
            .Name("FOLLOW_UP")
            .Description("Whether the Routine is a Follow Up or standard maintence.");
    }
}