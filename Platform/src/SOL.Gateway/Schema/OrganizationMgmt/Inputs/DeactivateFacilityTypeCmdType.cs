using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class DeactivateFacilityTypeCmdType : InputObjectType<DeactivateFacilityType>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeactivateFacilityType> descriptor)
    {
        descriptor
            .Name("DeactivateFacilityTypeCmd")
            .Description("The Command for deactivating a Facility Type.");

        descriptor
            .Field(x => x.FacilityTypeId)
            .Name("id")
            .Description("The unique identifier of the Facility Type to deactivate.");
    }
}