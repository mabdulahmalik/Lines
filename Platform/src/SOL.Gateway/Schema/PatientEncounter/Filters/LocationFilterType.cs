using HotChocolate.Data.Filters;
using SOL.Gateway.Views.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LocationFilterType : FilterInputType<LocationView>
{
    protected override void Configure(IFilterInputTypeDescriptor<LocationView> descriptor)
    {
        descriptor
            .Name("LocationFilter")
            .Description("The Facility/Room of an Encounter.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier for the Facility.");
        
        descriptor
            .Field(x => x.RoomId)
            .Name("roomId")
            .Description("The unique identifier for the Room within the Facility.");        
    }
}