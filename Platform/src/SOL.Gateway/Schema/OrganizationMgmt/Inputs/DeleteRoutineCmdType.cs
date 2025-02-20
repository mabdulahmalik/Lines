using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class DeleteRoutineCmdType : InputObjectType<DeleteRoutine>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeleteRoutine> descriptor)
    {
        descriptor
            .Name("DeleteRoutineCmd")
            .Description("The Command for deleting a Routine.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Routine to Delete.");        
    }
}