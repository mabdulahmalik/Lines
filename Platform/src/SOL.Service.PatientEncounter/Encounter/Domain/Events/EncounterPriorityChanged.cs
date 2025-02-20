using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterPriorityChanged))]
public record EncounterPriorityChanged : IMessage
{
    public Guid JobId { get; }
    public Guid EncounterId { get; }
    public EncounterPriority Priority { get; }
    public Object? Args { get; }

    public EncounterPriorityChanged(Encounter job, object? arg = null)
    {
        JobId = job.JobId;
        EncounterId = job.Id;
        Priority = job.Priority;
        Args = arg;
    }
}