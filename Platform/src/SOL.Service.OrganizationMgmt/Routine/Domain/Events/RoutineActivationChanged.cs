using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public record RoutineActivationChanged : IMessage
{
    public Guid RoutineId { get; }
    public bool IsActive { get; }

    internal RoutineActivationChanged(Guid routineId, bool isActive)
    {
        RoutineId = routineId;
        IsActive = isActive;
    }
}
