using HotChocolate.Data.Sorting;
using SOL.Service.OrganizationMgmt.FacilityRoom.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomsSorterType : SortInputType<FacilityRoomView>
{
    protected override void Configure(ISortInputTypeDescriptor<FacilityRoomView> descriptor)
    {
        descriptor
            .Name("FacilityRoomsSorter")
            .Description("Sorts the Rooms Query Results.");
        
        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Room.");
        
        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the facility was created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the facility was last modified.");               
    }
}