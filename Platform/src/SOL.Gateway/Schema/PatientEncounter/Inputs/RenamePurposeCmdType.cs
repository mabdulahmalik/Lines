using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RenamePurposeCmdType : InputObjectType<RenamePurpose>
{
    protected override void Configure(IInputObjectTypeDescriptor<RenamePurpose> descriptor)
    {
        descriptor
            .Name("RenamePurposeCmd")
            .Description("The Command that renames an existing Job Purpose.");

        descriptor
            .Field(t => t.PurposeId)
            .Name("id")
            .Description("The unique identifier of the Purpose to rename.");

        descriptor
            .Field(t => t.Name)
            .Name("name")
            .Description("The new Name/Descriptor of the Job Purpose.");
    }
}