using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ModifyRoutineCmdType : InputObjectType<ModifyRoutine>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyRoutine> descriptor)
    {
        descriptor
            .Name("ModifyRoutineCmd")
            .Description("The Command to modify a Routine.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Routine.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Routine.");

        descriptor
            .Field(x => x.Description)
            .Name("description")
            .Description("The Description of the Routine.");

        descriptor
            .Field(x => x.PurposeId)
            .Name("purposeId")
            .Description("The unique identifier of the Job Purpose to be created by this Routine.");

        descriptor
            .Field(t => t.Rolling)
            .Type<RollingSchedulePrmType>()
            .Name("rolling")
            .Description("Indicates the 'Rolling Interval' style Schedule in which the Routine will be executed.");

        descriptor
            .Field(t => t.Recurrence)
            .Type<ListType<RecurrenceSchedulePrmType>>()
            .Name("recurrence")
            .Description("Indicates the 'Recurrence' style Schedule in which the Routine will be executed.");

        descriptor
            .Field(t => t.Origins)
            .Type<ListType<RoutineTriggerPrmType>>()
            .Name("origins")
            .Description("The Procedure Triggers that START the Routine.");

        descriptor
            .Field(t => t.Termini)
            .Type<ListType<RoutineTriggerPrmType>>()
            .Name("termini")
            .Description("The Procedure Triggers that TERMINATE the Routine.");
    }
}