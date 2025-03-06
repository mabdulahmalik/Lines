using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record LineFacilityRoomChanged : IMessage
{
    public Guid LineId { get; init; }
    public Guid FacilityRoomId { get; init; }
}