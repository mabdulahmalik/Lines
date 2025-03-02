using SOL.Gateway.Schema.Common;
using SOL.Gateway.Views.OrganizationMgmt.Facility;
using SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;
using SOL.Gateway.Views.OrganizationMgmt.FacilityType;
using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class QueryFacilitiesExtensions : ObjectTypeExtension<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("facilityTypes")
            .Description("Gets all Facility Types.")
            .UseOffsetPaging<FacilityTypeType>()
            .UseProjection()
            .UseFiltering<FacilityTypesFilterType>()
            .ResolveWith<QueryResolver<FacilityTypeView>>(x => x.OrderedResults(default!));
        
        descriptor
            .Field("routines")
            .Description("Gets all Routines.")
            .UseOffsetPaging<RoutineType>()
            .UseProjection()
            .UseFiltering<RoutinesFilterType>()
            .UseSorting<RoutinesSorterType>()
            .ResolveWith<QueryResolver<RoutineView>>(x => x.Results(default!));
        
        descriptor
            .Field("facilities")
            .Description("Gets all Facilities.")
            .UseOffsetPaging<FacilityType>()
            .UseProjection()
            .UseFiltering<FacilitiesFilterType>()
            .ResolveWith<QueryResolver<FacilityView>>(x => x.OrderedResults(default!));
        
        descriptor
            .Field("facilityRooms")
            .Description("Gets all Rooms.")
            .UseOffsetPaging<FacilityRoomType>()
            .UseProjection()
            .UseFiltering<FacilityRoomsFilterType>()
            .UseSorting<FacilityRoomsSorterType>()
            .ResolveWith<QueryResolver<FacilityRoomView>>(x => x.Results(default!));
    }
}