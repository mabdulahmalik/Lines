using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ArchivePurpose : IMessage
{
    public Guid Id { get; init; }
}