using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.Common;

public record ObjectSorted : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Position { get; init; }
}