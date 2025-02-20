using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyValueType : ObjectType<FacilityRoomPropertyValueView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityRoomPropertyValueView> descriptor)
    {
        descriptor
            .Name("FacilityRoomPropertyValue")
            .Description("A Room Property assignment for a Facility.");

        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier of the Room Property.");

        descriptor
            .Field(x => x.OptionId)
            .Name("value")
            .Description("The unique identifier of the Room Property Value.");
    }
}