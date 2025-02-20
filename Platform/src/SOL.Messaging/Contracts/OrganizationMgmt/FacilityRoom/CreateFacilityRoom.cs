using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record CreateFacilityRoom : IMessage
{
    public string Name { get; init; }
    public Guid FacilityId { get; init; }
    public List<FacilityRoomProperty> Properties { get; init; } = new();
}

public record FacilityRoomProperty
{
    public Guid PropertyId { get; init; }
    public Guid OptionId { get; init; }
}
