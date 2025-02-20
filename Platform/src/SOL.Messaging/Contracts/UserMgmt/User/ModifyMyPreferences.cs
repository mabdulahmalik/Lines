using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ModifyMyPreferences : IMessage
{
    public List<UserPreference> Preferences { get; init; }
}
