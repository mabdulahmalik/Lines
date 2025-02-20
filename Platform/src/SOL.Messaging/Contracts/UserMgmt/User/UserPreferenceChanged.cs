using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record UserPreferenceChanged : IMessage
{
    public UserPreference Preferences { get; init; }
    public Guid UserId { get; init; }
}