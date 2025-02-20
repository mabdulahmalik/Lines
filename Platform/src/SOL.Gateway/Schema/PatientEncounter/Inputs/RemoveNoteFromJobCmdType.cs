using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RemoveNoteFromJobCmdType : InputObjectType<RemoveNoteFromJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<RemoveNoteFromJob> descriptor)
    {
        descriptor
            .Name("RemoveNoteFromJobCmd")
            .Description("The Command that removes a Note from it's Job.");

        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Note.");
    }
}