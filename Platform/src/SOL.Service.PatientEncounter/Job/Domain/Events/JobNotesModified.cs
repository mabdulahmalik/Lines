using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Job.Domain;

[TrackableActivity(nameof(JobNotesModified))]
public record JobNotesModified : IMessage
{
    public Guid JobId { get; }
    public IReadOnlyList<Note> Notes { get; }

    internal JobNotesModified(Job job, IEnumerable<Note> modifiedNotes)
    {
        JobId = job.Id;
        Notes = modifiedNotes.ToList();
    }
}