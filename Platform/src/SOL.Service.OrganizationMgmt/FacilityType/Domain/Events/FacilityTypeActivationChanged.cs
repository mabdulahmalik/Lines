using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public record FacilityTypeActivationChanged : IMessage
{
    public Guid TypeId { get; }
    public bool IsActive { get; }

    public FacilityTypeActivationChanged(Guid typeId, bool isActive)
    {
        TypeId = typeId;
        IsActive = isActive;
    }
}