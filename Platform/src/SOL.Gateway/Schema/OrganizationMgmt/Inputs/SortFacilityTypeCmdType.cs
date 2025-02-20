using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class SortFacilityTypeCmdType : InputObjectType<SortFacilityType>
{
    protected override void Configure(IInputObjectTypeDescriptor<SortFacilityType> descriptor)
    {
        descriptor
            .Name("SortFacilityTypeCmd")
            .Description("The Command for sorting a Facility Type.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility Type.");
        
        descriptor
            .Field(x => x.From)
            .Name("from")
            .Description("The Original Position of the Facility Type.");
        
        descriptor
            .Field(x => x.To)
            .Name("to")
            .Description("The Current Position of the Facility Type.");
    }
}