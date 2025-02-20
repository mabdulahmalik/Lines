using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RenamePurpose : IMessage
{
    public Guid PurposeId { get; init; }
    public string Name { get; init; }
}
