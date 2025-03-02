using AutoMapper;
using SOL.Service.UserMgmt.Invitation.Domain.Events;
using SE = SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Service.UserMgmt.User;

public class InvitationEventRelay : Profile
{
    public InvitationEventRelay()
    {
        CreateMap<UserInvitationsSent, SE.UserInvitationsSent>()
            .ForMember(x => x.Emails, x => x.MapFrom(y => y.Emails));

        CreateMap<UserInvitationRevoked, SE.UserInvitationRevoked>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id));

        CreateMap<UserInvitationResent, SE.UserInvitationResent>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id));
    }
}