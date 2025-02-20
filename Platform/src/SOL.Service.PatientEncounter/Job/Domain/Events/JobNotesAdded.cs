using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Job.Domain;

[TrackableActivity(nameof(JobNotesAdded))]
public record JobNotesAdded : IMessage
{
    public Guid JobId { get; }
    public IReadOnlyList<Note> Notes { get; }

    internal JobNotesAdded(Job job, IEnumerable<Note> addedNotes)
    {
        JobId = job.Id;
        Notes = addedNotes.ToList();
    }
}