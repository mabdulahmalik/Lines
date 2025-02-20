using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record AttachNoteToJob : IMessage
{
    public Guid JobId { get; init; }
    public string Text { get; init; }
    public bool IsPinned { get; init; }
    public Guid? MedicalRecordId { get; init; }
}