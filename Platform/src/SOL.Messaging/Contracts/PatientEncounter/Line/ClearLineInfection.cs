using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ClearLineInfection : IMessage
{
    public Guid Id { get; init; }
}