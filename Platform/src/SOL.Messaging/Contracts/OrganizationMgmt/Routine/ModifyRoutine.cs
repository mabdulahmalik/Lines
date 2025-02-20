using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ModifyRoutine : IMessage
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public Guid PurposeId { get; init; }
    public bool IsFollowUp { get; init; }
    public RollingSchedule Rolling { get; init; }
    public List<RecurrenceSchedule> Recurrence { get; init; } = new();
    public List<RoutineTrigger> Origins { get; init; } = new();
    public List<RoutineTrigger> Termini { get; init; } = new();
}
