using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineOriginType : ObjectType<RoutineOriginView>
{
    protected override void Configure(IObjectTypeDescriptor<RoutineOriginView> descriptor)
    {
        descriptor
            .Name("RoutineOrigin")
            .Description("A Procedure Trigger to START the Routine.");

        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier of the Procedure Property.");

        descriptor
            .Field(x => x.ProcedureId)
            .Name("procedureId")
            .Description("The unique identifier for the Procedure.");

        descriptor
            .Field(x => x.PropertyValue)
            .Name("propertyValue")
            .Description("The value for the Procedure Property.");
    }
}