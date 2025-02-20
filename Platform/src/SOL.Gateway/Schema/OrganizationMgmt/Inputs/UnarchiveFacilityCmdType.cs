using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class UnarchiveFacilityCmdType : InputObjectType<UnarchiveFacility>
{
    protected override void Configure(IInputObjectTypeDescriptor<UnarchiveFacility> descriptor)
    {
        descriptor
            .Name("UnarchiveFacilityCmd")
            .Description("The Command for Unarchiving a Facility.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("id")
            .Description("The unique identifier of the Facility to Unarchive.");            
    }
}