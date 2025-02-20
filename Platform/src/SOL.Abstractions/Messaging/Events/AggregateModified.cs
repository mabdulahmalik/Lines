using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Abstractions.Domain;

[TrackableActivity(nameof(AggregateModified))]
public record AggregateModified : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}