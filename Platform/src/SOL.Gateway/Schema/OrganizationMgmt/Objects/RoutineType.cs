using SOL.Abstractions.Domain;
using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineType : ObjectType<RoutineView>
{
    protected override void Configure(IObjectTypeDescriptor<RoutineView> descriptor)
    {
        descriptor
            .Name("Routine")
            .Description("A Routine definition.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Routine.");

        descriptor
            .Field(x => x.PurposeId)
            .Name("purposeId")
            .Description("The unique identifier of the Job Purpose that will be created by the Routine.");

        descriptor
            .Field(x => x.FollowUp)
            .Name("followUp")
            .Description("Whether or not the Routine is a Follow Up.");            

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Routine.");

        descriptor
            .Field(x => x.Description)
            .Name("description")
            .Description("The Description of the Routine.");

        descriptor
            .Field(x => x.Active)
            .Name("active")
            .Description("Whether the Routine is active.");
        
        descriptor
            .Field(x => x.AssignmentCount)
            .Name("assignmentCount")
            .Description("The number of Assignments to Facilities.");

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Routine was Created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Routine was last Modified.");

        descriptor
            .Field("rolling")
            .Description("The Rolling Schedule for the Routine.")
            .Type<RollingScheduleType>()
            .Resolve(async ctx => await ctx.DataLoader<RollingScheduleLoader>()
                .LoadAsync(ctx.Parent<RoutineView>().Id, ctx.RequestAborted));

        descriptor
            .Field("recurrence")
            .Description("A list of Scheduled Recurrences for the Routine.")
            .Type<ListType<RecurrenceScheduleType>>()
            .Resolve(async ctx => await ctx.DataLoader<RecurrenceScheduleLoader>()
                .LoadAsync(ctx.Parent<RoutineView>().Id, ctx.RequestAborted));
        
        descriptor
            .Field("origin")
            .Description("A list of Triggers to START the Routine.")
            .Type<ListType<RoutineOriginType>>()
            .Resolve(async ctx => await ctx.DataLoader<RoutineOriginLoader>()
                .LoadAsync(ctx.Parent<RoutineView>().Id, ctx.RequestAborted));

        descriptor
            .Field("termini")
            .Description("A list of Triggers to TERMINATE the Routine.")
            .Type<ListType<RoutineTerminiType>>()
            .Resolve(async ctx => await ctx.DataLoader<RoutineTerminiLoader>()
                .LoadAsync(ctx.Parent<RoutineView>().Id, ctx.RequestAborted));            
    }
}