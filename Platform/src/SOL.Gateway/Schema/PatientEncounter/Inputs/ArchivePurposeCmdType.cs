using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ArchivePurposeCmdType : InputObjectType<ArchivePurpose>
{
    protected override void Configure(IInputObjectTypeDescriptor<ArchivePurpose> descriptor)
    {
        descriptor
            .Name("ArchivePurposeCmd")
            .Description("Archives a Purpose by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Purpose.");    
    }
}