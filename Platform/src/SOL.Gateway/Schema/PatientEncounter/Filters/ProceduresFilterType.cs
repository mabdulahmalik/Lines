using HotChocolate.Data.Filters;
using SOL.Gateway.Views.PatientEncounter.Procedure;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProceduresFilterType : FilterInputType<ProcedureView>
{
    protected override void Configure(IFilterInputTypeDescriptor<ProcedureView> descriptor)
    {
        descriptor
            .Name("ProceduresFilter")
            .Description("Filters for the Procedures Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Procedure.");

        descriptor
            .Field(x => x.Settings)
            .Name("settings")
            .Description("The Settings of the Procedure.");

        descriptor
            .Field(x => x.RequiredData)
            .Name("requiredData")
            .Description("The required Patient data for the Procedure.");

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Procedure was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Procedure was modified.");
        
        descriptor
            .Field(x => x.Archived)
            .Name("archived")
            .Description("Whether the Procedure is Archived.");
        
        descriptor
            .Field(x => x.References)
            .Name("references")
            .Description("The number of objects that reference this Procedure.");           
    }
}