using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record WithdrawMeFromEncounter : IMessage
{
    public Guid EncounterId { get; init; }
}
