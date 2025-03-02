using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.UserMgmt.Invitation.Domain.Events;

public record UserInvitationsSent : IMessage
{
    public IEnumerable<string> Emails { get; }

    internal UserInvitationsSent(IEnumerable<string> emails)
    {
        Emails = emails;
    }
}