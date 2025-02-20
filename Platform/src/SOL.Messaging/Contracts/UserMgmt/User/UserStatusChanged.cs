using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record UserStatusChanged : IMessage
{
    public UserAvailability Status { get; init; }
    public Instant ChangedAt { get; init; }
    public string? Message { get; init; }
    public Guid UserId { get; init; }
}
