using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record MakeNoteAnObservation : IMessage
{
    public Guid Id { get; init; }
    public Guid JobId { get; init; }
    public Guid MedicalRecordId { get; init; }
}