using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MakeNoteAnObservationCmdType : InputObjectType<MakeNoteAnObservation>
{
    protected override void Configure(IInputObjectTypeDescriptor<MakeNoteAnObservation> descriptor)
    {
        descriptor
            .Name("MakeNoteAnObservationCmd")
            .Description("The Command that turns a Note into an Observation.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Note.");
        
        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job that owns the Note.");
        
        descriptor
            .Field(t => t.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The unique identifier of the Medical Record for the Observation.");        
    }
}