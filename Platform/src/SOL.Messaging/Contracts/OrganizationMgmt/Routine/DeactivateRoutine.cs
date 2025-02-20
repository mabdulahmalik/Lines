using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record DeactivateRoutine : IMessage
{
    public Guid RoutineId { get; init; }
}