using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record DeactivateUser : IMessage
{
    public Guid UserId { get; set; }
}