using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class SortPurposeCmdType : InputObjectType<SortPurpose>
{
    protected override void Configure(IInputObjectTypeDescriptor<SortPurpose> descriptor)
    {
        descriptor
            .Name("SortPurposeCmd")
            .Description("Explicitly sorts a Purpose.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Purpose.");
        
        descriptor
            .Field(x => x.From)
            .Name("from")
            .Description("The Original Position of the Purpose.");
        
        descriptor
            .Field(x => x.To)
            .Name("to")
            .Description("The Current Position of the Purpose.");         
    }
}