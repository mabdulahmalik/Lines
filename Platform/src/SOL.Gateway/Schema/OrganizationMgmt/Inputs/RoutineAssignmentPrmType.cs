using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineAssignmentPrmType : InputObjectType<RoutineAssignment>
{
    protected override void Configure(IInputObjectTypeDescriptor<RoutineAssignment> descriptor)
    {
        descriptor
            .Name("RoutineAssignmentPrm")
            .Description("The Parameters for the Facility Routine Assignment.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Routine Assignment.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Routine Assignment.");

        descriptor
            .Field(x => x.RoutineId)
            .Name("routineId")
            .Description("The unique identifier of the Routine Assignment.");

        descriptor
            .Field(x => x.Specs)
            .Type<ListType<RoutineAssignmentSpecPrmType>>()
            .Name("specs")
            .Description("The list of the Specs of the Routine Assignment.");
    }
}