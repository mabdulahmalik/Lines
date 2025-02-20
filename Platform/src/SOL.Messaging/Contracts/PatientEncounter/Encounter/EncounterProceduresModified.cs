using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterProceduresModified : IMessage
{
    public Guid EncounterId { get; init; }
    public IEnumerable<EncounterProcedure> Procedures { get; init; }
}
