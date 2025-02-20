using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CancelAssistanceRequestForEncounter : IMessage
{
    public Guid EncounterId { get; init; }
}
