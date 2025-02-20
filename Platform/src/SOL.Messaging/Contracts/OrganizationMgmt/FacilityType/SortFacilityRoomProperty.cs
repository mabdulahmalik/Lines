using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record SortFacilityRoomProperty : IMessage
{
    public Guid FacilityTypeId { get; init; }
    public Guid Id { get; init; }
    public int From { get; init; }
    public int To { get; init; }    
}