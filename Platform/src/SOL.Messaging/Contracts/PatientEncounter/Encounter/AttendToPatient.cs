using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record AttendToPatient : IMessage
{
    public Guid EncounterId { get; init; }
}