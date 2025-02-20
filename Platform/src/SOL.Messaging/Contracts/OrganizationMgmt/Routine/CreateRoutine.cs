using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record CreateRoutine : IMessage
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public Guid PurposeId { get; init; }
    public bool IsFollowUp { get; init; }
    public RollingSchedule Rolling { get; init; }
    public List<RecurrenceSchedule> Recurrence { get; init; } = new();
    public List<RoutineTrigger> Origins { get; init; } = new();
    public List<RoutineTrigger> Termini { get; init; } = new();
}

public record RollingSchedule
{
    public RollingScheduleDuration Interval { get; init; }
    public RollingScheduleDuration Delay { get; init; }
}

public record RollingScheduleDuration
{
    public int Value { get; init; }
    public DurationUnit Unit { get; init; }
}

public record RecurrenceSchedule
{
    public List<IsoDayOfWeek> Days { get; init; } = new();
    public LocalTime Time { get; init; }
}

public class RoutineTrigger
{
    public Guid ProcedureId { get; init; }
    public Guid? PropertyId { get; init; }
    public string? PropertyValue { get; init; }
}