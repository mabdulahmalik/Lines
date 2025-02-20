using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RemoveNoteFromJob : IMessage
{
    public Guid Id { get; init; }
    public Guid JobId { get; init; }
}
