using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ApplyProcedureToEncounter : IMessage
{
    public Guid EncounterId { get; init; }
    public Guid ProcedureId { get; init; }
    public List<EncounterProcedureValue> Values { get; init; } = new();
    public List<Guid> RoutinesAssigned { get; init; }
    public List<Guid> RoutinesRemoved { get; init; }
    public string? InsertedLineName { get; init; }
    public Guid? RemovedLineId { get; init; }
}
public record EncounterProcedureValue
{
    public Guid FieldId { get; init; }
    public string Value { get; init; }
}
