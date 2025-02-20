using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CompleteEncounter : IMessage
{
    public Guid EncounterId { get; init; }
}