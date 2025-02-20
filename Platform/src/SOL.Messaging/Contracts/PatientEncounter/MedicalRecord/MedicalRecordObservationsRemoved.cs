using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record MedicalRecordObservationsRemoved : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<MedicalRecordObservation> Observations { get; init; } = new List<MedicalRecordObservation>();    
}