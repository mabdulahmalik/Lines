using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ActivateUser : IMessage
{
    public Guid UserId { get; set; }
}
