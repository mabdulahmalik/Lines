using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class SortFacilityCmdType : InputObjectType<SortFacility>
{
    protected override void Configure(IInputObjectTypeDescriptor<SortFacility> descriptor)
    {
        descriptor
            .Name("SortFacilityCmd")
            .Description("The Command for sorting a Facility.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility.");
        
        descriptor
            .Field(x => x.From)
            .Name("from")
            .Description("The Original Position of the Facility.");
        
        descriptor
            .Field(x => x.To)
            .Name("to")
            .Description("The Current Position of the Facility.");          
    }
}