using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterPhotosDetached : IMessage
{
    public Guid EncounterId { get; init; }
    public IEnumerable<Guid> PhotoIds { get; init; } = [];
}
