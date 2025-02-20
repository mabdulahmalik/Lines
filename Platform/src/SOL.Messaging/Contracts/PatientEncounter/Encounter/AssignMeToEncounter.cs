using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record AssignMeToEncounter : IMessage
{
    public Guid EncounterId { get; init; }
    public Guid FacilityRoomId { get; init; }
    public Guid? MedicalRecordId { get; init; }
}
