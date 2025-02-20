using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class CreatePurposeCmdType : InputObjectType<CreatePurpose>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreatePurpose> descriptor)
    {
        descriptor
            .Name("CreatePurposeCmd")
            .Description("The Command that holds the data needed to create a new Purpose.");

        descriptor
            .Field(t => t.Name)
            .Name("name")
            .Description("The Name/Descriptor of the Job Purpose.");

        descriptor
            .Field(t => t.InsertOnTop)
            .Name("insertOnTop")
            .Description("Whether to insert the new Purpose at the top or bottom of the existing Purposes.")
            .DefaultValue(false);
    }
}