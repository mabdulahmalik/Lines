using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record UnarchiveFacility : IMessage
{
    public Guid FacilityId { get; init; }
}