using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ActivateFacilityTypeCmdType : InputObjectType<ActivateFacilityType>
{
    protected override void Configure(IInputObjectTypeDescriptor<ActivateFacilityType> descriptor)
    {
        descriptor
            .Name("ActivateFacilityTypeCmd")
            .Description("The Command for activating a Facility Type.");

        descriptor
            .Field(x => x.FacilityTypeId)
            .Name("id")
            .Description("The unique identifier of the Facility Type to Activate.");
    }
}
