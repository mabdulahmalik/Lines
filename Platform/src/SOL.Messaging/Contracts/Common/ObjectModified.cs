using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.Common;

public record ObjectModified : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}