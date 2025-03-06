using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ModifyFacilityTypeCmdType : InputObjectType<ModifyFacilityType>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyFacilityType> descriptor)
    {
        descriptor
            .Name("ModifyFacilityTypeCmd")
            .Description("The Command for modifying a Facility Type.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility Type.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Facility Type.");
        
        descriptor
            .Field(x => x.Active)
            .Name("active")
            .Description("Whether or not the Facility Type is active.");
        
        descriptor
            .Field(x => x.Properties)
            .Type<ListType<FacilityRoomPropertyPrmType>>()
            .Name("properties")
            .Description("The name of the Facility Type.");
    }
}