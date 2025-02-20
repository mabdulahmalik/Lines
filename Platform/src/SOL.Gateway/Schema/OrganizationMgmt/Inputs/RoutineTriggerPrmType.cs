using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineTriggerPrmType : InputObjectType<RoutineTrigger>
{
    protected override void Configure(IInputObjectTypeDescriptor<RoutineTrigger> descriptor)
    {
        descriptor
            .Name("RoutineTriggerPrm")
            .Description("A trigger for Routine.");

        descriptor
            .Field(x => x.ProcedureId)
            .Name("procedureId")
            .Description("The unique identifier for the Procedure.");        
        
        descriptor
            .Field(x => x.PropertyId)
            .Name("propertyId")
            .Description("The unique identifier for Property.");

        descriptor
            .Field(x => x.PropertyValue)
            .Name("propertyValue")
            .Description("The property value");
    }
}