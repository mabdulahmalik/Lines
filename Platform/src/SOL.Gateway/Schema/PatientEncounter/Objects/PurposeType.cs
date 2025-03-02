using SOL.Gateway.Views.PatientEncounter.Purpose;

namespace SOL.Gateway.Schema.PatientEncounter;

public class PurposeType : ObjectType<PurposeView>
{
    protected override void Configure(IObjectTypeDescriptor<PurposeView> descriptor)
    {
        descriptor
            .Name("Purpose")
            .Description("The Purpose of a Job.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Purpose.")
            .IsProjected();

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