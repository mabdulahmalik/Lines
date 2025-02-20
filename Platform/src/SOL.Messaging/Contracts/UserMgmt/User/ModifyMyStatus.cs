using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ModifyMyStatus : IMessage
{
    public UserAvailability Status { get; set; }
    public string? Message { get; set; }
}
