using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class AssignMeToEncounterCmdType : InputObjectType<AssignMeToEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<AssignMeToEncounter> descriptor)
    {
        descriptor
            .Name("AssignMeToEncounterCmd")
            .Description("The Command that assigns the active user to the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");
        
        descriptor
            .Field(t => t.FacilityRoomId)
            .Name("facilityRoomId")
            .Description("The unique identifier of the Room of the Patient.");
        
        descriptor
            .Field(t => t.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The unique identifier of the Medical Record of the Patient.");           
    }
}


