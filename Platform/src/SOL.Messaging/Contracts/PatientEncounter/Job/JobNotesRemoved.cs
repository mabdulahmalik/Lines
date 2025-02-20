using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record JobNotesRemoved : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<Guid> NoteIds { get; init; } = new List<Guid>();
}
