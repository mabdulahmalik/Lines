using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record FacilityRoomPropertySorted : IMessage
{
    public Guid Id { get; init; }
    public Guid FacilityTypeId { get; init; }
    public int Position { get; init; }
}