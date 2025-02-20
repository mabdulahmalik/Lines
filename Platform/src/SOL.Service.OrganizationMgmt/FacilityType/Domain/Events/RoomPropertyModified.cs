using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public record RoomPropertyModified : IMessage
{
    public Guid FacilityTypeId { get; }
    public RoomProperty Property { get; }

    public RoomPropertyModified(Guid facilityTypeId, RoomProperty property)
    {
        FacilityTypeId = facilityTypeId;
        Property = property;
    }
}
