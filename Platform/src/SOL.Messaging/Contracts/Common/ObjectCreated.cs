using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.Common;

public record ObjectCreated : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}