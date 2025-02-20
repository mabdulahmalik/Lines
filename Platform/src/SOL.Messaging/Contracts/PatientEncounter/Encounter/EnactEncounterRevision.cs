using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EnactEncounterRevision : IMessage
{
    public Guid JobId { get; init; }
    public Guid? EncounterId { get; init; }
    public string? Contact { get; init; }
    public string? OrderingProvider { get; init; }
    public IntakeLocation Location { get; init; }
    public ModifyMedicalRecord MedicalRecord { get; init; }
}