using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record UserActivationChanged : IMessage
{
    public Guid UserId { get; init; }
    public bool IsActive { get; init; }
}