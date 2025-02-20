using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Job.Domain;

public class ActiveRoutine : ValueType<ActiveRoutine>
{
    public Instant Timestamp { get; }
    public Guid EncounterProcedureId { get; }
    public Guid? RoutineAssignmentId { get; }
    
    internal ActiveRoutine(Instant timestamp, Guid encounterProcedureId, Guid? routineAssignmentId)
    {
        Timestamp = timestamp;
        EncounterProcedureId = encounterProcedureId;
        RoutineAssignmentId = routineAssignmentId;
    }
    
    internal ActiveRoutine(Guid encounterProcedureId, Guid? routineAssignmentId)
        : this(SystemClock.Instance.GetCurrentInstant(), encounterProcedureId, routineAssignmentId)
    { }

    protected override bool EqualsInternal(ActiveRoutine? other)
    {
        return Timestamp.Equals(other?.Timestamp)
            && EncounterProcedureId.Equals(other?.EncounterProcedureId)
            && RoutineAssignmentId.Equals(other?.RoutineAssignmentId);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Timestamp, EncounterProcedureId, RoutineAssignmentId);
    }
}