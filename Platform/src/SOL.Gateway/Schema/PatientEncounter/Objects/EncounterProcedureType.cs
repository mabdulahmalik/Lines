using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureType : ObjectType<EncounterProcedureView>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterProcedureView> descriptor)
    {
        descriptor
            .Name("EncounterProcedure")
            .Description("A Procedure performed on a Patient, during an Encounter.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the Procedure.");

        descriptor
            .Field(t => t.ProcedureId)
            .Name("procedureId")
            .Description("The unique identifier for the Procedure.");

        descriptor
            .Field(t => t.PerformedAt)
            .Name("performAt")
            .Description("The date and time the Procedure was performed.");

        descriptor
            .Field(t => t.PerformedBy)
            .Name("performedBy")
            .Description("The unique identifier of the user that performed the Procedure.");

        descriptor
            .Field("values")
            .Description("The values of the Procedure per this Encounter.")
            .Type<ListType<EncounterProcedureValueType>>()
            .Resolve(async ctx => await ctx.DataLoader<EncounterProcedureValueLoader>()
                .LoadAsync(ctx.Parent<EncounterProcedureView>().Id, ctx.RequestAborted));
    }
}