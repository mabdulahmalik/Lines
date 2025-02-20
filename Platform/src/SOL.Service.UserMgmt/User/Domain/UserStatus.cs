using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.UserMgmt.User.Domain;

public class UserStatus : ValueType<UserStatus>
{
    public Guid UserId { get; }
    public UserAvailability Availability { get; }
    public Instant ChangedAt { get; }
    public string? Message { get; }

    internal UserStatus(Guid userId, Instant changedAt, UserAvailability availability, string? message)
    {
        UserId = userId;
        Availability = availability;
        Message = message;
        ChangedAt = changedAt;
    }
    
    internal UserStatus(Guid userId, UserAvailability availability, string? message = null)
        : this(userId, SystemClock.Instance.GetCurrentInstant(), availability, message)
    {  }

    protected override bool EqualsInternal(UserStatus? other)
    {
        return Availability.Equals(other?.Availability)
               && ChangedAt.Equals(other?.ChangedAt)
               && String.Compare(Message, other?.Message, StringComparison.CurrentCultureIgnoreCase) == 0;
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Availability, ChangedAt, Message);
    }
}
