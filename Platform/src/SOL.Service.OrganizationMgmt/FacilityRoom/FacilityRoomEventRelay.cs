using AutoMapper;
using SOL.Service.OrganizationMgmt.FacilityType.Domain;
using SE = SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Service.OrganizationMgmt.FacilityRoom;

public class FacilityRoomEventRelay : Profile
{
    public FacilityRoomEventRelay()
    {
        CreateMap<RoomPropertyOption, SE.FacilityRoomPropertyOption>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Text, y => y.MapFrom(z => z.Text));
        
        CreateMap<RoomPropertyAdded, SE.FacilityRoomPropertyAdded>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Property.Id))
            .ForMember(x => x.FacilityTypeId, x => x.MapFrom(y => y.FacilityTypeId))
            .ForMember(x => x.Name, y => y.MapFrom(z => z.Property.Name))
            .ForMember(x => x.Position, y => y.MapFrom(z => z.Property.Order))
            .ForMember(x => x.Options, y => y.MapFrom(z => z.Property.Options));
        
        CreateMap<RoomPropertyModified, SE.FacilityRoomPropertyModified>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Property.Id))
            .ForMember(x => x.FacilityTypeId, x => x.MapFrom(y => y.FacilityTypeId))
            .ForMember(x => x.Name, y => y.MapFrom(z => z.Property.Name))
            .ForMember(x => x.Options, y => y.MapFrom(z => z.Property.Options));        
        
        CreateMap<RoomPropertySorted, SE.FacilityRoomPropertySorted>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.PropertyId))
            .ForMember(x => x.FacilityTypeId, x => x.MapFrom(y => y.FacilityTypeId))
            .ForMember(x => x.Position, y => y.MapFrom(z => z.To));
    }
}