using MassTransit;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ModifyMyAvatar : IMessage
{
    public string FileName { get; init; }
    public MessageData<Stream> Avatar { get; init; }
}