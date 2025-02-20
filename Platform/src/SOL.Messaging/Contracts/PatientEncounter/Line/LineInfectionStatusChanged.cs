using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record LineInfectionStatusChanged : IMessage
{
    public Guid Id { get; init; }
    public bool HasInfection { get; init; }
    public LocalDate? InfectedOn { get; init; }
}