using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class CreateFacilityRoomCmdType : InputObjectType<CreateFacilityRoom>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateFacilityRoom> descriptor)
    {
        descriptor
            .Name("CreateFacilityRoomCmd")
            .Description("The Command for creating a new Room for Facility.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Room.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier of the Facility.");

        descriptor
            .Field(x => x.Properties)
            .Type<ListType<FacilityRoomPropertyValuePrmType>>()
            .Name("properties")
            .Description("The properties of the Facility.");
    }
}