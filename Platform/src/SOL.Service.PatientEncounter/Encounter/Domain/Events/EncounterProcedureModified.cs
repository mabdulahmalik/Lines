using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterProcedureModified))]
public record EncounterProcedureModified : IMessage
{
    public Guid EncounterId { get; }
    public Procedure Procedure { get; }

    public EncounterProcedureModified(Encounter job, Procedure modifiedProcedure)
    {
        EncounterId = job.Id;
        Procedure = modifiedProcedure;
    }
}