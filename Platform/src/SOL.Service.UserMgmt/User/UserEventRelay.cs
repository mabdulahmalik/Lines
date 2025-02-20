using AutoMapper;
using SOL.Service.UserMgmt.User.Domain.Events;
using SE = SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Service.UserMgmt.User;

public class UserEventRelay : Profile
{
    public UserEventRelay()
    {
        CreateMap<UserStatusChanged, SE.UserStatusChanged>()
            .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.ChangedAt, y => y.MapFrom(z => z.ChangedAt))
            .ForMember(x => x.Status, y => y.MapFrom(z => z.Status))
            .ForMember(x => x.Message, y => y.MapFrom(z => z.Message));

        CreateMap<UserPreferenceChanged, SE.UserPreferenceChanged>()
            .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Preferences, y => y.MapFrom(z => z.Preferences));

        CreateMap<UserActivationChanged, SE.UserActivationChanged>()
            .ForMember(x => x.UserId, x => x.MapFrom(y => y.UserId))
            .ForMember(x => x.IsActive, y => y.MapFrom(z => z.IsActive));
    }
}