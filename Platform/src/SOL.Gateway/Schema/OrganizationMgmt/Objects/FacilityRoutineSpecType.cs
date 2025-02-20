using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoutineSpecType : ObjectType<FacilityRoutineSpecView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityRoutineSpecView> descriptor)
    {
        descriptor
            .Name("FacilityRoutineSpec")
            .Description("A Routine Assignment Specification for a Facility.");
        
        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier of the Room Property.");

        descriptor
            .Field(x => x.PropertyValue)
            .Name("propertyValue")
            .Description("The unique identifier of the Room Property Value.");
    }
}