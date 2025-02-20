using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class AttendToPatientCmdType : InputObjectType<AttendToPatient>
{
    protected override void Configure(IInputObjectTypeDescriptor<AttendToPatient> descriptor)
    {
        descriptor
            .Name("AttendToPatientCmd")
            .Description("The Command that advances the Encounter workflow into the active most state.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");        
    }
}