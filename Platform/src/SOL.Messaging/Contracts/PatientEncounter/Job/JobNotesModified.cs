using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record JobNotesModified : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<JobNote> Notes { get; init; } = new List<JobNote>();
}
