using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Job.Domain;

[TrackableActivity(nameof(JobNotesRemoved))]
public record JobNotesRemoved : IMessage
{
    public Guid JobId { get; }
    public IReadOnlyList<Guid> NoteIds { get; }

    internal JobNotesRemoved(Job job, IEnumerable<Note> removedNotes)
    {
        JobId = job.Id;
        NoteIds = removedNotes.Select(n => n.Id).ToList();
    }
}