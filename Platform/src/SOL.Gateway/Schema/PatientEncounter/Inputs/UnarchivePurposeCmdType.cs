using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class UnarchivePurposeCmdType : InputObjectType<UnarchivePurpose>
{
    protected override void Configure(IInputObjectTypeDescriptor<UnarchivePurpose> descriptor)
    {
        descriptor
            .Name("UnarchivePurposeCmd")
            .Description("Unarchives a Purpose by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Purpose.");
    }
}