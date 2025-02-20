using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ArchiveProcedure : IMessage
{
    public Guid Id { get; init; }
}