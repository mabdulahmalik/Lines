using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RecordLineInfection : IMessage
{
    public Guid Id { get; init; }
    public LocalDate InfectedOn { get; init; }
}