using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public record RoomPropertyAdded : IMessage
{
    public Guid FacilityTypeId { get; }
    public RoomProperty Property { get; }

    public RoomPropertyAdded(Guid facilityTypeId, RoomProperty property)
    {
        FacilityTypeId = facilityTypeId;
        Property = property;
    }
}
