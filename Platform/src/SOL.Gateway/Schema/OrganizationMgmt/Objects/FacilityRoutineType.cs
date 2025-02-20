using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoutineType : ObjectType<FacilityRoutineView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityRoutineView> descriptor)
    {
        descriptor
            .Name("FacilityRoutine")
            .Description("A Routine Assignment to a Facility.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Routine Assignment.");
        
        descriptor
            .Field(x => x.RoutineId)
            .Name("routineId")
            .Description("The unique identifier of the Routine.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Routine Assignment.");

        descriptor
            .Field("specs")
            .Description("The list of Routine Assignment specs.")
            .Type<ListType<FacilityRoutineSpecType>>()
            .Resolve(async ctx => await ctx.DataLoader<FacilityRoutineSpecLoader>()
                .LoadAsync(ctx.Parent<FacilityRoutineView>().Id, ctx.RequestAborted));
    }
}