using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record RenameFacilityType : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}