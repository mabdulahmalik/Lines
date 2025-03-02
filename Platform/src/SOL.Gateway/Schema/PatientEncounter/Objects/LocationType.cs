using SOL.Gateway.Views.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LocationType : ObjectType<LocationView>
{
    protected override void Configure(IObjectTypeDescriptor<LocationView> descriptor)
    {
        descriptor
            .Name("Location")
            .Description("A physical Location.");
        
        descriptor
            .Field(t => t.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier of the Facility.");
        
        descriptor
            .Field(t => t.RoomId)
            .Name("roomId")
            .Description("The unique identifier of the Room.");        
    }
}