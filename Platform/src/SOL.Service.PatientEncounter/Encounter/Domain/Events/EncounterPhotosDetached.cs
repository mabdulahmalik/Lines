using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public record EncounterPhotosDetached : IMessage
{
    public Guid EncounterId { get; }
    public IReadOnlyList<Photo> Photos { get; }

    public EncounterPhotosDetached(Encounter job, IEnumerable<Photo> detachedPhotos)
    {
        EncounterId = job.Id;
        Photos = detachedPhotos.ToList();
    }
}