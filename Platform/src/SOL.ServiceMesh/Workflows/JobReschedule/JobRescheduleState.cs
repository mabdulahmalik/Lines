using MassTransit;

namespace SOL.ServiceMesh.Workflows.JobReschedule;

public class JobRescheduleState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid JobId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
    public string? Reason { get; set; }
}