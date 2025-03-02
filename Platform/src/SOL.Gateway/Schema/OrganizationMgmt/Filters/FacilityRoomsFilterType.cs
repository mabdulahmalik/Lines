using HotChocolate.Data.Filters;
using SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomsFilterType : FilterInputType<FacilityRoomView>
{
    protected override void Configure(IFilterInputTypeDescriptor<FacilityRoomView> descriptor)
    {
        descriptor
            .Name("FacilityRoomsFilter")
            .Description("Filters the Rooms Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Room.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier of the Facility.");

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