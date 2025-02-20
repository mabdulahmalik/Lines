using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ActivateFacilityType : IMessage
{
    public Guid FacilityTypeId { get; set; }
}
