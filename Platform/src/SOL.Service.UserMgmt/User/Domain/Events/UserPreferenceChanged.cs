using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.UserMgmt.User.Domain.Events;

public record UserPreferenceChanged : IMessage
{
    public UserPreference Preferences { get; }
    public Guid Id { get; }
    
    internal UserPreferenceChanged(Guid id, UserPreference preference)
    {
        Id = id;
        Preferences = preference;    
    }
}