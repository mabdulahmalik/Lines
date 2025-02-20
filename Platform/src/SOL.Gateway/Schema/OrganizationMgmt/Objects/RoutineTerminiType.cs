using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineTerminiType : ObjectType<RoutineTerminiView>
{
    protected override void Configure(IObjectTypeDescriptor<RoutineTerminiView> descriptor)
    {
        descriptor
            .Name("RoutineTermini")
            .Description("A Procedure Trigger to TERMINATE the Routine.");

        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier of the Procedure Property.");

        descriptor
            .Field(x => x.ProcedureId)
            .Name("procedureId")
            .Description("The unique identifier of the Procedure.");

        descriptor
            .Field(x => x.PropertyValue)
            .Name("propertyValue")
            .Description("The value of the Procedure Property.");
    }
}