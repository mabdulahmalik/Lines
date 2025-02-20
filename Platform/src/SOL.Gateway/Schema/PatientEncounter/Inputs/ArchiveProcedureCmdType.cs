using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ArchiveProcedureCmdType : InputObjectType<ArchiveProcedure>
{
    protected override void Configure(IInputObjectTypeDescriptor<ArchiveProcedure> descriptor)
    {
        descriptor
            .Name("ArchiveProcedureCmd")
            .Description("Archives a Procedure by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.");        
    }
}