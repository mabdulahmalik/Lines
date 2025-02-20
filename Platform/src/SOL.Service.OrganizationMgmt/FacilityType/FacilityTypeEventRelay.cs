using AutoMapper;
using SOL.Service.OrganizationMgmt.FacilityType.Domain;
using SE = SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Service.OrganizationMgmt.FacilityType;

public class FacilityTypeEventRelay : Profile
{
    public FacilityTypeEventRelay()
    {
        CreateMap<FacilityTypeActivationChanged, SE.FacilityTypeActivationChanged>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.TypeId))
            .ForMember(x => x.IsActive, y => y.MapFrom(z => z.IsActive));
    }
}