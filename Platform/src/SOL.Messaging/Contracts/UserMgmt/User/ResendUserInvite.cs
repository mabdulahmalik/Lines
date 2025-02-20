using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ResendUserInvite : IMessage
{
    public Guid InviteId { get; set; }
}