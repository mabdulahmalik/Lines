using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record CreateFacilityRoomProperty : IMessage
{
    public Guid FacilityTypeId { get; init; }
    public string Name { get; init; }
    public IEnumerable<string> Options { get; init; } = new List<string>();
}