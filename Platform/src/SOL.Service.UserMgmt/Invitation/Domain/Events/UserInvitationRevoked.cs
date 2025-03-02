using SOL.Abstractions.Messaging;

namespace SOL.Service.UserMgmt.Invitation.Domain.Events;

public record UserInvitationRevoked : IMessage
{
    public Guid Id { get; }

    internal UserInvitationRevoked(Guid id)
    {
        Id = id;
    }
}
