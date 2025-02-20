using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record MedicalRecordObservationsAdded : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<MedicalRecordObservation> Observations { get; init; } = new List<MedicalRecordObservation>();
}

public record MedicalRecordObservation
{
    public Guid ObjectId { get; init; }
    public Instant Timestamp { get; init; }
    public ObservationType Type { get; init; }
}