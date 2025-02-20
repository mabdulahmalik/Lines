using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(CliniciansWithdrawn))]
public class CliniciansWithdrawn : IMessage
{
    public Guid EncounterId { get; }
    public IReadOnlyList<Assignment> Assignments { get; }

    public CliniciansWithdrawn(Encounter job, IEnumerable<Assignment> removedAssignments)
    {
        EncounterId = job.Id;
        Assignments = removedAssignments.ToList();
    }
}