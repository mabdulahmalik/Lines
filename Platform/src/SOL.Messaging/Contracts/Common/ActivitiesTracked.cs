using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.Common;

public record ActivitiesTracked : IMessage
{
    public IReadOnlyList<TrackedActivity> Activities { get; init; }
}

public record TrackedActivity
{
    public Guid Id { get; init; }
    public string ActivityType { get; init; }
    public string AggregateType { get; init; }
    public Guid AggregateId { get; init; }
    public string Metadata { get; init; }
    public Guid UserId { get; init; }
    public Instant Timestamp { get; init; }
}