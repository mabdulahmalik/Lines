using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineEncounterAttached))]
public record LineEncounterAttached : IMessage
{
    internal LineEncounterAttached(Guid id, IEnumerable<Guid> encounterIds)
    {
        LineId = id;
        EncounterIds = encounterIds.ToList();
    }

    public Guid LineId { get; }
    public IReadOnlyList<Guid> EncounterIds { get; }
}