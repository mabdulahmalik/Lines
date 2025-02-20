using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ModifyFacilityRoom : IMessage
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public List<FacilityRoomProperty> Properties { get; init; } = new();
}
