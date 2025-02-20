using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RemoveHoldFromEncounter : IMessage
{
    public Guid EncounterId { get; init; }
}
