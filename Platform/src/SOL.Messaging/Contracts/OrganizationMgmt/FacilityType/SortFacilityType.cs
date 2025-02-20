using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record SortFacilityType : IMessage
{
    public Guid Id { get; init; }
    public int From { get; init; }
    public int To { get; init; }
}