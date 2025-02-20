using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterProgressed : IMessage
{
    public Guid EncounterId { get; init; }
    public EncounterStage Stage { get; init; }
    public Instant EnteredAt { get; init; }
    public int Duration { get; init; }
}
