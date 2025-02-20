using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public record RoutineTerminiCleared : IMessage
{
    public Guid RoutineId { get; }

    internal RoutineTerminiCleared(Guid routineId)         
    {
        RoutineId = routineId; 
    }
}
