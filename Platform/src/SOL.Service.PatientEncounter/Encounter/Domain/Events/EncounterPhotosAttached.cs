using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterPhotosAttached))]
public record EncounterPhotosAttached : IMessage
{
    public Guid EncounterId { get; }
    public Guid JobId { get; }
    public IReadOnlyList<Photo> Photos { get; }

    public EncounterPhotosAttached(Encounter encounter, IEnumerable<Photo> addedPhotos)
    {
        EncounterId = encounter.Id;
        JobId = encounter.JobId;
        Photos = addedPhotos.ToList();
    }
}