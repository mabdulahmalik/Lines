using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CreatePurpose : IMessage
{
    public string Name { get; init; }
    public bool InsertOnTop { get; init; }
}