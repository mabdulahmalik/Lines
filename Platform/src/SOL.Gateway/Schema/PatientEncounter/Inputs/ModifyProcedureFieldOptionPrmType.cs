using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ModifyProcedureFieldOptionPrmType : InputObjectType<ModifyProcedureFieldOption>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyProcedureFieldOption> descriptor)
    {
        descriptor
            .Name("ModifyProcedureFieldOptionPrm")
            .Description("The Parameters for a ProcedureField Option.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Option.");

        descriptor
            .Field(t => t.Text)
            .Name("text")
            .Description("The text for the Option.");
        
        descriptor
            .Field(t => t.Archived)
            .Name("archived")
            .Description("Whether or not the Option is Archived.");        
    }
}