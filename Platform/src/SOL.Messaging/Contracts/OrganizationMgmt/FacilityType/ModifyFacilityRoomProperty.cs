using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ModifyFacilityRoomProperty : IMessage
{
    public Guid Id { get; init; }
    public Guid FacilityTypeId { get; init; }
    public string Name { get; init; }
    public List<FacilityRoomPropertyOption> Options { get; init; } = new();
}

public record FacilityRoomPropertyOption
{
    public Guid? Id { get; init; }
    public string Text { get; init; }
}