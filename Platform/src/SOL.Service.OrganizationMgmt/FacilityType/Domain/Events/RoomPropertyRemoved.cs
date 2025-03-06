using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public record RoomPropertyRemoved : IMessage
{
    internal RoomPropertyRemoved(Guid facilityTypeId, RoomProperty roomProperty)
    {
        FacilityTypeId = facilityTypeId;
        Property = roomProperty;
    }

    public Guid FacilityTypeId { get; }
    public RoomProperty Property { get; }
}