using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EnactJobReschedule : IMessage
{
    public Guid Id { get; init; }
    public Guid FacilityId { get; init; }
    public string Reason { get; init; }
    public LocalDate Date { get; init; }
    public LocalTime? Time { get; init; }
}