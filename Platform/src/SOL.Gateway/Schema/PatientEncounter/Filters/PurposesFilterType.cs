using HotChocolate.Data.Filters;
using SOL.Service.PatientEncounter.Purpose.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class PurposesFilterType : FilterInputType<PurposeView>
{
    protected override void Configure(IFilterInputTypeDescriptor<PurposeView> descriptor)
    {
        descriptor
            .Name("PurposesFilter")
            .Description("Filters the Job Purpose Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Purpose.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Purpose.");

        descriptor
            .Field(x => x.Archived)
            .Name("archived")
            .Description("Whether the purpose is Archived.");

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Purpose was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Purpose was last modified.");
        
        descriptor
            .Field(x => x.References)
            .Name("references")
            .Description("The number of objects that reference this Purpose.");         
    }
}