using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterProceduresApplied))]
public record EncounterProceduresApplied : IMessage
{
    public Guid EncounterId { get; }
    public IReadOnlyList<Procedure> Procedures { get; }

    public EncounterProceduresApplied(Encounter job, IEnumerable<Procedure> appliedProcedures)
    {
        EncounterId = job.Id;
        Procedures = appliedProcedures.ToList();
    }
}