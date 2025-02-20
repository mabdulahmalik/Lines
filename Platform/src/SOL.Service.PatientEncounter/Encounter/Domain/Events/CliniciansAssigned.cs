using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(CliniciansAssigned))]
public record CliniciansAssigned : IMessage
{
    public Guid EncounterId { get; }
    public IReadOnlyList<Assignment> Assignments { get; }

    public CliniciansAssigned(Encounter job, IEnumerable<Assignment> addedAssignments)
    {
        EncounterId = job.Id;
        Assignments = addedAssignments.ToList();
    }
}