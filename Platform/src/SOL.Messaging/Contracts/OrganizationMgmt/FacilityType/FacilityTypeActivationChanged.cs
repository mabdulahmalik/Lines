using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record FacilityTypeActivationChanged : IMessage
{
    public Guid Id { get; init; }
    public bool IsActive { get; init; }
}