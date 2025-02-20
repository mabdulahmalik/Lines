using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public record RoomPropertySorted : IMessage
{
    internal RoomPropertySorted(Guid facilityTypeId, Guid propertyId, int from, int to)
    {
        FacilityTypeId = facilityTypeId;
        PropertyId = propertyId;
        From = from;
        To = to;
    }

    public Guid FacilityTypeId { get; }
    public Guid PropertyId { get; }
    public int From { get; }
    public int To { get; }
}