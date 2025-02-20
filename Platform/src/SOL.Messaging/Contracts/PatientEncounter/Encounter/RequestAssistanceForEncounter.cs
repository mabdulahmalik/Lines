using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RequestAssistanceForEncounter : IMessage
{
    public Guid EncounterId { get; init; }
    public string? Reason { get; init; }
}
