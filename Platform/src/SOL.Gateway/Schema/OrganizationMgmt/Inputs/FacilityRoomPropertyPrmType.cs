using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyPrmType : InputObjectType<FacilityRoomProperty>
{
    protected override void Configure(IInputObjectTypeDescriptor<FacilityRoomProperty> descriptor)
    {
        descriptor
            .Name("FacilityRoomPropertyPrm")
            .Description("The Room Property Parameters, used in Facility Room mutations.");

        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier of the Property.");

        descriptor
            .Field(x => x.OptionId)
            .Name("optionId")
            .Description("The unique identifier of the Property Value");
    }
}