using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterRoomChanged))]
public record EncounterRoomChanged : IMessage
{
    public Guid EncounterId { get; }
    public Guid JobId { get; }
    public Guid RoomId { get; }

    public EncounterRoomChanged(Encounter job)
    {
        EncounterId = job.Id;
        JobId = job.JobId;
        RoomId = job.FacilityRoomId;
    }
}