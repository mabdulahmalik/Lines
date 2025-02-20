using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineAssignmentSpecPrmType : InputObjectType<RoutineAssignmentSpec>
{
    protected override void Configure(IInputObjectTypeDescriptor<RoutineAssignmentSpec> descriptor)
    {
        descriptor
            .Name("RoutineAssignmentSpecPrm")
            .Description("Parameters for the Routine Assignment's specification, used when Mutating Routines.");

        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier of the Property.");

        descriptor
            .Field(x => x.PropertyValue)
            .Name("propertyValue")
            .Description("The unique identifier of the Property Value.");
    }
}