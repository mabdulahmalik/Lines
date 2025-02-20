using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record FacilityRoomPropertyAdded : IMessage
{
    public Guid Id { get; init; }
    public Guid FacilityTypeId { get; init; }
    public string Name { get; init; }
    public int Position { get; init; }
    public IReadOnlyList<FacilityRoomPropertyOption> Options { get; init; }
}