using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ModifyProcedureForEncounter : IMessage
{
    public Guid Id { get; init; }
    public Guid EncounterId { get; init; }
    public List<EncounterProcedureValue> Values { get; init; } = new();
}
