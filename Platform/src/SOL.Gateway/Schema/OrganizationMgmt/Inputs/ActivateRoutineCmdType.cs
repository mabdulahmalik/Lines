using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ActivateRoutineCmdType : InputObjectType<ActivateRoutine>
{
    protected override void Configure(IInputObjectTypeDescriptor<ActivateRoutine> descriptor)
    {
        descriptor
            .Name("ActivateRoutineCmd")
            .Description("The Command for activating a Routine.");

        descriptor
            .Field(x => x.RoutineId)
            .Name("id")
            .Description("The unique identifier of the Routine to Activate.");
    }
}