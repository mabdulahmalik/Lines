using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record RoutineActivationChanged : IMessage
{
    public Guid Id { get; init; }
    public bool Active { get; init; }
}