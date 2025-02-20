using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class DeleteJobCmdType : InputObjectType<DeleteJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeleteJob> descriptor)
    {
        descriptor
            .Name("DeleteJobCmd")
            .Description("Command that holds the Job Deletion details.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Job.");
    }
}