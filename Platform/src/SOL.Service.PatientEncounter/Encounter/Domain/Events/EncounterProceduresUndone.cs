using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterProceduresUndone))]
public record EncounterProceduresUndone : IMessage
{
    public Guid JobId { get; }
    public Guid EncounterId { get; }
    public IReadOnlyList<Procedure> Procedures { get; }

    public EncounterProceduresUndone(Encounter job, IEnumerable<Procedure> undoneProcedures)
    {
        JobId = job.JobId;
        EncounterId = job.Id;
        Procedures = undoneProcedures.ToList();
    }
}