using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineInfectionStatusChanged))]
public record LineInfectionStatusChanged : IMessage
{
    internal LineInfectionStatusChanged(Guid id, LocalDate? infectedOn)
    {
        LineId = id;
        InfectedOn = infectedOn;
        HasInfection = infectedOn.HasValue;
    }

    public Guid LineId { get; }
    public bool HasInfection { get; }
    public LocalDate? InfectedOn { get; }
}