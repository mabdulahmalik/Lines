using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class IntakeLocationPrmType : InputObjectType<IntakeLocation>
{
    protected override void Configure(IInputObjectTypeDescriptor<IntakeLocation> descriptor)
    {
        descriptor
            .Name("IntakeLocationPrm")
            .Description("Holds the location data for the Job Intake process.");        
        
        descriptor
            .Field(t => t.FacilityId)
            .Name("facilityId")
            .Description("The Facility Location of the job.");

        descriptor
            .Field(t => t.RoomId)
            .Name("roomId")
            .Description("The Room within the Facility of the job.");

        descriptor
            .Field(t => t.RoomName)
            .Name("roomName")
            .Description("The name of the Room within the Facility of the job.");
    }
}