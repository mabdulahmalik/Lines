using SOL.Gateway.Views.PatientEncounter.Procedure;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldOptionType : ObjectType<ProcedureFieldOptionView>
{
    protected override void Configure(IObjectTypeDescriptor<ProcedureFieldOptionView> descriptor)
    {
        descriptor
            .Name("ProcedureFieldOption")
            .Description("An option for a ProcedureField, that is of type `List`.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Option.");

        descriptor
            .Field(x => x.Text)
            .Name("text")
            .Description("The text/value of the Option.");
        
        descriptor
            .Field(x => x.Archived)
            .Name("archived")
            .Description("Whether the Option is Archived.");
        
        descriptor
            .Field(x => x.References)
            .Name("references")
            .Description("The number of objects that reference this Option.");            
    }
}