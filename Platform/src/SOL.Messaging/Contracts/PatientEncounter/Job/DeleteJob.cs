using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record DeleteJob : IMessage
{
    public Guid Id { get; init; }
}