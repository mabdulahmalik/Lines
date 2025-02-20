using AutoMapper;
using SOL.Service.OrganizationMgmt.Routine.Domain;
using SE = SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Service.OrganizationMgmt.Routine;

public class RoutineEventRelay : Profile
{
    public RoutineEventRelay()
    {
        CreateMap<RoutineActivationChanged, SE.RoutineActivationChanged>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.RoutineId))
            .ForMember(x => x.Active, x => x.MapFrom(y => y.IsActive));
    }
}