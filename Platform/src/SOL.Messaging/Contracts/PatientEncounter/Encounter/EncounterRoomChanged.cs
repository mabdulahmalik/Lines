using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterRoomChanged : IMessage
{
    public Guid EncounterId { get; init; }
    public Guid JobId { get; init; }
    public Guid RoomId { get; init; }
}