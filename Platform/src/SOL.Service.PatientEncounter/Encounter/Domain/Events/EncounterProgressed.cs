using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterProgressed))]
public record EncounterProgressed : IMessage
{
    public Guid JobId { get; }
    public Guid EncounterId { get; }
    public EncounterStage Stage { get; }
    public int Duration { get; }
    public Object? Args { get; }

    internal EncounterProgressed(Encounter job, int duration, object? arg = null)
    {
        JobId = job.JobId;
        EncounterId = job.Id;
        Stage = job.Stage;
        Duration = duration;
        Args = arg;
    }
}