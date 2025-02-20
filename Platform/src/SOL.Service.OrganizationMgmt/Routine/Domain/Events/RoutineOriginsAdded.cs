using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public record RoutineOriginsAdded : IMessage
{
    public Guid RoutineId { get; }
    public IReadOnlyList<Trigger> Origins { get; }

    internal RoutineOriginsAdded(Guid routineId, IEnumerable<Trigger> origins)
    {
        RoutineId = routineId;
        Origins = origins.ToList();
    }
}
