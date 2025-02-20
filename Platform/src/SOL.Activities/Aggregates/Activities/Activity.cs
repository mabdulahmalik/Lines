using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Activities.Aggregates.Activities;

public class Activity(Guid id)
{
    public Guid Id { get; protected set; } = id;
    public string ActivityType { get; set; }
    public string AggregateType { get; set; }
    public Guid AggregateId { get; set; }
    public string Metadata { get; set; }
    public Guid UserId { get; set; }
    public Instant Timestamp { get; set; }
}