using SOL.Abstractions.Messaging;

namespace SOL.Abstractions.Domain;

public record AggregateRestored : IMessage
{
    public IReadOnlyList<Guid> Ids { get; init; } = new List<Guid>();
    public string Name { get; init; }
}