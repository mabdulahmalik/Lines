using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineFacilityRoomChanged))]
public record LineFacilityRoomChanged : IMessage
{
    internal LineFacilityRoomChanged(Guid id, Guid facilityRoomId)
    {
        LineId = id;
        FacilityRoomId = facilityRoomId;
    }

    public Guid LineId { get; }
    public Guid FacilityRoomId { get; }
}