using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ActivateScheduledJobs : IMessage
{
    public IEnumerable<Guid> FacilityIds { get; init; }
    public LocalDate Date { get; init; }
    public Instant RunTime { get; init; }
    public string TimeZone { get; init; }
}