using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterPriorityChanged : IMessage
{
    public Guid EncounterId { get; init; }
    public EncounterPriority Priority { get; init; }
}
