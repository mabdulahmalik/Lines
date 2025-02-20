using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class UnpinNoteFromJobCmdType : InputObjectType<UnpinNoteFromJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<UnpinNoteFromJob> descriptor)
    {
        descriptor
            .Name("UnpinNoteFromJobCmd")
            .Description("The Command that UNPINS a Note from it's Job.");

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