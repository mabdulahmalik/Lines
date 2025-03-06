using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public class ModifyFacilityType : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool Active { get; init; }
    public List<FacilityRoomProperty> Properties { get; init; } = new();
}

public record FacilityRoomProperty
{
    public Guid? Id { get; init; }
    public string Name { get; init; }
    public List<FacilityRoomPropertyOption> Options { get; init; } = new();
}

public record FacilityRoomPropertyOption
{
    public Guid? Id { get; init; }
    public string Text { get; init; }
}