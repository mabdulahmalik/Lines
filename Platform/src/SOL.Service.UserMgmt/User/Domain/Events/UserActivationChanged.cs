using SOL.Abstractions.Messaging;

namespace SOL.Service.UserMgmt.User.Domain.Events;

public record UserActivationChanged : IMessage
{
    public Guid UserId { get; }
    public bool IsActive { get; }

    internal UserActivationChanged(Guid userId, bool isActive)
    {
        UserId = userId;
        IsActive = isActive;
    }
}