using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record CancelUserInvite : IMessage
{
    public Guid InviteId { get; set; }
}