using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public record RoutineTerminiAdded : IMessage
{
    public Guid RoutineId { get; }
    public IReadOnlyList<Trigger> Termini { get; }

    internal RoutineTerminiAdded(Guid routineId, IEnumerable<Trigger> termini)
    {
        RoutineId = routineId;
        Termini = termini.ToList();
    }
}
