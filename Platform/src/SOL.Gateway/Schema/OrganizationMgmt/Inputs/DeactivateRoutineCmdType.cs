using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class DeactivateRoutineCmdType : InputObjectType<DeactivateRoutine>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeactivateRoutine> descriptor)
    {
        descriptor
            .Name("DeactivateRoutineCmd")
            .Description("The Command for deactivating a Routine.");

        descriptor
            .Field(x => x.RoutineId)
            .Name("id")
            .Description("The unique identifier of the Routine to Deactivate.");
    }    
}