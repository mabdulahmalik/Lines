using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class DeleteFacilityCmdType : InputObjectType<DeleteFacility>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeleteFacility> descriptor)
    {
        descriptor
            .Name("DeleteFacilityCmd")
            .Description("The Command for deleting a Facility.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility to Delete.");           
    }
}