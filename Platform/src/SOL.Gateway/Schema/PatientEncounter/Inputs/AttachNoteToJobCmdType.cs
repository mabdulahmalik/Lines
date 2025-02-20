using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class AttachNoteToJobCmdType : InputObjectType<AttachNoteToJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<AttachNoteToJob> descriptor)
    {
        descriptor
            .Name("AttachNoteToJobCmd")
            .Description("The Command that attaches a Note to the Job.");

        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job.");

        descriptor
            .Field(t => t.Text)
            .Name("text")
            .Description("The Text for the Note.");
        
        descriptor
            .Field(t => t.IsPinned)
            .Name("pinned")
            .Description("Whether or not the Note is Pinned and displayed on the card of the Job and Encounter.")
            .DefaultValue(false);
        
        descriptor
            .Field(t => t.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The unique identifier of the Medical Record. Specifying this value will create an Observation out of this Note.");        
    }
}


