using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Abstractions.Domain;

[TrackableActivity(nameof(AggregateDeleted))]
public record AggregateDeleted : IMessage
{
    public IReadOnlyList<Guid> Ids { get; init; } = new List<Guid>();
    public string Name { get; init; }
}