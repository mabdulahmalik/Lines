using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record UserInvitationResent : IMessage
{
    public Guid Id { get; init; }
}