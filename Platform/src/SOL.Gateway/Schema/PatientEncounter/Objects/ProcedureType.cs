using SOL.Abstractions.Domain;
using SOL.Gateway.Schema.Common;
using SOL.Gateway.Views.PatientEncounter.Procedure;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureType : ObjectType<ProcedureView>
{
    protected override void Configure(IObjectTypeDescriptor<ProcedureView> descriptor)
    {
        descriptor
            .Name("Procedure")
            .Description("A Procedure definition.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.")
            .IsProjected();

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Procedure.");
        
        descriptor
            .Field(x => x.Type)
            .Type<ProcedureTypeType>()
            .Name("type")
            .Description("The Type of the Procedure.");
        
        descriptor
            .Field(x => x.Archived)
            .Name("archived")
            .Description("Whether the Procedure is Archived.");
        
        descriptor
            .Field(x => x.References)
            .Name("references")
            .Description("The number of objects that reference this Procedure.");         

        descriptor
            .Field(x => x.Settings)
            .Type<ListType<ProcedureSettingType>>()
            .Name("settings")
            .Description("The Settings of the Procedure.")
            .Resolve(ctx => {
                var procedure = ctx.Parent<ProcedureView>();
                return Enum.GetValues<ProcedureSetting>().Where(e => procedure.Settings.HasFlag(e));
            });

        descriptor
            .Field(x => x.RequiredData)
            .Type<ListType<RequiredPatientDataType>>()
            .Name("requiredData")
            .Description("The required Patient data for the Procedure.")
            .Resolve(ctx => {
                var procedure = ctx.Parent<ProcedureView>();
                return Enum.GetValues<RequiredPatientData>().Where(e => procedure.RequiredData.HasFlag(e));
            });

        descriptor
            .Field("fields")
            .Description("The Fields/data points the Procedure captures.")
            .Type<ListType<ProcedureFieldType>>()
            .Resolve(async ctx => await ctx.DataLoader<ProcedureFieldLoader>()
                .LoadAsync(ctx.Parent<ProcedureView>().Id, ctx.RequestAborted));

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Procedure was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Procedure was modified.");
    }
}