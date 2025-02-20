using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterProceduresUndone : IMessage
{
    public Guid EncounterId { get; init; }
    public IEnumerable<Guid> Ids { get; init; }
}
