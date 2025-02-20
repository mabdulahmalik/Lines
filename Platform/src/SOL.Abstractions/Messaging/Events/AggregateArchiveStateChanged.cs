namespace SOL.Abstractions.Messaging;

public record AggregateArchiveStateChanged : IMessage
{
    public IReadOnlyList<Guid> Ids { get; init; } = new List<Guid>();
    public string Name { get; init; }
    public bool Archived { get; init; }
}