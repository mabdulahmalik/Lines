using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record DeletePurpose : IMessage
{
    public Guid Id { get; init; }
}