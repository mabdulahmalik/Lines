using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class CancelJobCmdType : InputObjectType<CancelJob>
{
    protected override void Configure(IInputObjectTypeDescriptor<CancelJob> descriptor)
    {
        descriptor
            .Name("CancelJobCmd")
            .Description("Command that holds the Job Cancellation details.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Job.");

        descriptor
            .Field(t => t.Reason)
            .Name("reason")
            .Description("The Reason for Job being Canceled.");        
    }
}