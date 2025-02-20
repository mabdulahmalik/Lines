using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ActivateRoutine : IMessage
{
    public Guid RoutineId { get; init; }
}