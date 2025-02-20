using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ModifyFacilityRoomCmdType : InputObjectType<ModifyFacilityRoom>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyFacilityRoom> descriptor)
    {
        descriptor
            .Name("ModifyFacilityRoomCmd")
            .Description("The Command to modify a Room.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Room.");

        descriptor
            .Field(x => x.Properties)
            .Type<ListType<FacilityRoomPropertyPrmType>>()
            .Name("properties")
            .Description("The list of Properties of the Room.");
    }
}
