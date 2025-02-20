using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class DeletePurposeCmdType : InputObjectType<DeletePurpose>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeletePurpose> descriptor)
    {
        descriptor
            .Name("DeletePurposeCmd")
            .Description("Deletes a Purpose by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Purpose.");
    }
}