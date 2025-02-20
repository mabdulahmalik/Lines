using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public record RoutineOriginsCleared : IMessage
{
    public Guid RoutineId { get; }

    internal RoutineOriginsCleared(Guid routineId)
    {
        RoutineId = routineId;
    }
}
