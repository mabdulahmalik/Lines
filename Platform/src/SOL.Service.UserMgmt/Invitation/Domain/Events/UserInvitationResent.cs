using SOL.Abstractions.Messaging;

namespace SOL.Service.UserMgmt.Invitation.Domain.Events;

public record UserInvitationResent : IMessage
{
    public Guid Id { get; }

    internal UserInvitationResent(Guid id)
    {
        Id = id;
    }
}