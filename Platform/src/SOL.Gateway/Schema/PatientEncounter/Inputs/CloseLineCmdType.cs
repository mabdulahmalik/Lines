using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class CloseLineCmdType : InputObjectType<CloseLine>
{
    protected override void Configure(IInputObjectTypeDescriptor<CloseLine> descriptor)
    {
        descriptor
            .Name("CloseLineCmd")
            .Description("The Command that closes/removes a Line.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");

        descriptor
            .Field(t => t.RemovedOn)
            .Name("removedOn")
            .Description("The date when the Line was removed.");
    }
}