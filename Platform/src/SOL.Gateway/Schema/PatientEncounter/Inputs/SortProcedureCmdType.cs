using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class SortProcedureCmdType : InputObjectType<SortProcedure>
{
    protected override void Configure(IInputObjectTypeDescriptor<SortProcedure> descriptor)
    {
        descriptor
            .Name("SortProcedureCmd")
            .Description("Explicitly sorts a Procedure.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.");
        
        descriptor
            .Field(x => x.From)
            .Name("from")
            .Description("The Original Position of the Procedure.");
        
        descriptor
            .Field(x => x.To)
            .Name("to")
            .Description("The Current Position of the Procedure.");  
    }
}