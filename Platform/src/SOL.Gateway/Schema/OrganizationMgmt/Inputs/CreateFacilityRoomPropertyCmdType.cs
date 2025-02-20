using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class CreateFacilityRoomPropertyCmdType : InputObjectType<CreateFacilityRoomProperty>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateFacilityRoomProperty> descriptor)
    {
        descriptor
            .Name("CreateFacilityRoomPropertyCmd")
            .Description("The Command for creating a Room Property for a Facility Type.");

        descriptor
            .Field(x => x.FacilityTypeId)
            .Name("facilityTypeId")
            .Description("The unique identifier of the Facility Type.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the room property.");

        descriptor
            .Field(x => x.Options)
            .Name("options")
            .Description("Options for the room property.")
            .Type<ListType<StringType>>();
    }
}