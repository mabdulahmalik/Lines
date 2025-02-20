using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class PinNoteToJobCmdType : InputObjectType<PinNoteToJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<PinNoteToJob> descriptor)
    {
        descriptor
            .Name("PinNoteToJobCmd")
            .Description("The Command that PINS a Note to it's Job.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Note.");
        
        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job that owns the Note.");        
    }
}