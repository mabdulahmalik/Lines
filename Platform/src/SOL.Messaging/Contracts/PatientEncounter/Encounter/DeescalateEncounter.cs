using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record DeescalateEncounter : IMessage
{
    public Guid EncounterId { get; init; }
}