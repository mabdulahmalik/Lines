using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record UnarchivePurpose : IMessage
{
    public Guid Id { get; init; }
}