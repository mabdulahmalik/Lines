using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RemovePhotoFromEncounter : IMessage
{
    public Guid EncounterId { get; init; }
    public Guid Id { get; init; }
}