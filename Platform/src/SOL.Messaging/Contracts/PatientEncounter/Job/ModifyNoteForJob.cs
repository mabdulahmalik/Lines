using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ModifyNoteForJob : IMessage
{
    public Guid Id { get; init; }
    public Guid JobId { get; init; }
    public string Text { get; init; }
}
