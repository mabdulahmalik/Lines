using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record DeleteFacility : IMessage
{
    public Guid Id { get; init; }
}