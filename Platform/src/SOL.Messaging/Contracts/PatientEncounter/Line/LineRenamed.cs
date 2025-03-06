using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record LineRenamed : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
