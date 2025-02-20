using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class SortFacilityRoomPropertyCmdType : InputObjectType<SortFacilityRoomProperty>
{
    protected override void Configure(IInputObjectTypeDescriptor<SortFacilityRoomProperty> descriptor)
    {
        descriptor
            .Name("SortFacilityRoomPropertyCmd")
            .Description("The Command for sorting a Room Property.");
        
        descriptor
            .Field(x => x.FacilityTypeId)
            .Name("facilityTypeId")
            .Description("The unique identifier of the Facility Type.");        

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room Property.");
        
        descriptor
            .Field(x => x.From)
            .Name("from")
            .Description("The Original Position of the Room Property.");
        
        descriptor
            .Field(x => x.To)
            .Name("to")
            .Description("The Current Position of the Room Property.");        
    }
}