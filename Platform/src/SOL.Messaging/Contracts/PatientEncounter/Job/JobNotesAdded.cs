using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record JobNotesAdded : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<JobNote> Notes { get; init; } = new List<JobNote>();
}

public record JobNote
{
    public Guid Id { get; init; }
    public string Text { get; init; }
    public Guid CreatedBy { get; init; }
    public Instant CreatedAt { get; init; }
    public JobNoteTreatment Treatment { get; init; }
}