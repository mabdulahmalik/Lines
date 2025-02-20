using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CancelJob : IMessage
{
    public Guid Id { get; init; }
    public string? Reason { get; init; }
}