using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record JobRescheduled : IMessage
{
    public Guid Id { get; init; }
    public string? Reason { get; init; }
    public Guid? ScheduledBy { get; init; }
    public LocalDate ScheduledDate { get; init; }
    public LocalTime? ScheduledTime { get; init; }
}