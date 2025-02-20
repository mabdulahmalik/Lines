using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ArchiveFacility : IMessage
{
    public Guid FacilityId { get; init; }
}