using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record UserInvitationsSent : IMessage
{
    public IEnumerable<string> Emails { get; init; }
}