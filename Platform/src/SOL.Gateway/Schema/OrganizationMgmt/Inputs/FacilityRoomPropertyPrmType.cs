using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyPrmType : InputObjectType<FacilityRoomProperty>
{
    protected override void Configure(IInputObjectTypeDescriptor<FacilityRoomProperty> descriptor)
    {
        descriptor
            .Name("FacilityRoomPropertyPrm")
            .Description("The Parameters to modify a Room Property for a Facility Type.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room Property.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Room Property.");

        descriptor
            .Field(x => x.Options)
            .Name("options")
            .Description("Options for the Room Property.")
            .Type<ListType<FacilityRoomPropertyOptionPrmType>>();
    }
}