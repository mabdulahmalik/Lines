using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record DeactivateFacilityType : IMessage
{
    public Guid FacilityTypeId { get; set; }
}