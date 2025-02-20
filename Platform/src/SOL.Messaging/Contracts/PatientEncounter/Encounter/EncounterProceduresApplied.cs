using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterProceduresApplied : IMessage
{
    public Guid EncounterId { get; init; }
    public IEnumerable<EncounterProcedure> Procedures { get; init; }
}

public record EncounterProcedure
{
    public Guid Id { get; init; }
    public Guid ProcedureId { get; init; }
    public Guid PerformedBy { get; init; }
    public Instant PerformedAt { get; init; }
    public IEnumerable<EncounterProcedureValue> Values { get; init; }
}