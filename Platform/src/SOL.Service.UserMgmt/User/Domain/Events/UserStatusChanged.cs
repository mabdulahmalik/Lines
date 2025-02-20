using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.UserMgmt.User.Domain.Events;

public record UserStatusChanged : IMessage
{
    public UserAvailability Status { get; }
    public Instant ChangedAt { get; }
    public string? Message { get; }
    public Guid Id { get; }

    internal UserStatusChanged(UserStatus status)
    {
        Status = status.Availability;
        ChangedAt = status.ChangedAt;
        Message = status.Message;
        Id = status.UserId;
    }
}