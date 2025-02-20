using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record CreateFacilityType : IMessage
{
    public string Name { get; init; }
}