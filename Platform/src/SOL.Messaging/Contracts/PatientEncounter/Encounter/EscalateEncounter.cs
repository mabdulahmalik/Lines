using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EscalateEncounter : IMessage
{
    public Guid EncounterId { get; init; }
    public string? Reason { get; init; }
}
