using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record InviteUsers : IMessage
{
    public List<string> Emails { get; set; } = new();
    public List<Guid> Roles { get; set; } = new();
}
