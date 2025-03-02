using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record UserInvitationRevoked : IMessage
{
    public Guid Id { get; init; }
}
