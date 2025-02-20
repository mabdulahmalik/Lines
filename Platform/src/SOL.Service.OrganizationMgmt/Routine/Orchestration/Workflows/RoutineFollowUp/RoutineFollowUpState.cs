using MassTransit;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Workflows.RoutineFollowUp;

public class RoutineFollowUpState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid PurposeId { get; set; }
    public Guid RoomId { get; set; }
    public Guid FacilityId { get; set; }
    public DateOnly ScheduledDate { get; set; }
    public TimeOnly? ScheduledTime { get; set; }
    
    public bool IsRoutineActive { get; set; }
    public bool IsScheduledForToday => ScheduledDate == DateOnly.FromDateTime(DateTime.Today);
}