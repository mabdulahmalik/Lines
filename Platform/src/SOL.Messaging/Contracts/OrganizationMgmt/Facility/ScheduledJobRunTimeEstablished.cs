using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ScheduledJobRunTimeEstablished : IMessage
{
    public IEnumerable<Guid> FacilityIds { get; init; }
    public LocalDate Date { get; init; }
    public Instant RunTime { get; init; }
    public string TimeZone { get; init; }
    public Guid JobId { get; init; }
}