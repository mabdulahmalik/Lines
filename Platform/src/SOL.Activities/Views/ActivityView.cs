using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Activities.Views;

public class ActivityView
{
    public Guid? Id { get; set; }
    public string? ActivityType { get; set; }
    public string? AggregateType { get; set; }
    public Guid? AggregateId { get; set; }
    public string? Metadata { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? Timestamp { get; set; }
}