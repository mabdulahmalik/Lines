using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CreateEncounter : IMessage
{
    public Guid JobId { get; init; }
    public Guid PurposeId { get; init; }
    public Guid FacilityRoomId { get; init; }
    public bool IsStat { get; init; } = false;
    public bool IsOnHold { get; init; } = false;
    public string? StatReason { get; init; }
    public string? HoldReason { get; init; }
}