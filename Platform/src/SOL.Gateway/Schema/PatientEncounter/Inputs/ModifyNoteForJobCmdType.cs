using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ModifyNoteForJobCmdType : InputObjectType<ModifyNoteForJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyNoteForJob> descriptor)
    {
        descriptor
            .Name("ModifyNoteForJobCmd")
            .Description("The Command that modifies a Note for the Job.");

        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Note.");

        descriptor
            .Field(t => t.Text)
            .Name("text")
            .Description("The Text for the Note.");
    }
}