using SOL.Abstractions.Domain;
using SOL.Service.PatientEncounter.Procedure.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldType : ObjectType<ProcedureFieldView>
{
    protected override void Configure(IObjectTypeDescriptor<ProcedureFieldView> descriptor)
    {
        descriptor
            .Name("ProcedureField")
            .Description("A Field (data point) for a Procedure.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Field.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Field.");

        descriptor
            .Field(x => x.Instruction)
            .Name("instruction")
            .Description("The Instructions for the Field.");            

        descriptor
            .Field(x => x.Type)
            .Type<ProcedureFieldTypeType>()
            .Name("type")
            .Description("The Data Type of the Field.");
        
        descriptor
            .Field(x => x.Archived)
            .Name("archived")
            .Description("Whether the Field is Archived.");
        
        descriptor
            .Field(x => x.References)
            .Name("references")
            .Description("The number of objects that reference this Field.");            

        descriptor
            .Field(x => x.Settings)
            .Type<ListType<ProcedureFieldSettingType>>()
            .Name("settings")
            .Description("The Settings for the Field.")
            .Resolve(ctx => {
                var field = ctx.Parent<ProcedureFieldView>();
                return Enum.GetValues<ProcedureFieldSetting>().Where(e => field.Settings.HasFlag(e));
            });

        descriptor
            .Field("options")
            .Description("The List Options for the Field that is of type 'List'.")
            .Type<ListType<ProcedureFieldOptionType>>()
            .Resolve(async ctx => await ctx.DataLoader<ProcedureFieldOptionLoader>()
                .LoadAsync(ctx.Parent<ProcedureFieldView>().Id, ctx.RequestAborted));
    }
}