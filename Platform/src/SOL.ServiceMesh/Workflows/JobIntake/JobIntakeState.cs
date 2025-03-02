using MassTransit;

namespace SOL.ServiceMesh.Workflows.JobIntake;

public class JobIntakeState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid RoomId { get; set; }
    
    public Guid PurposeId { get; set; }
    public string? Contact { get; set; }
    public string? OrderingProvider { get; set; }
    public string? PreNote { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public TimeOnly? ScheduledTime { get; set; }
    
    public Guid FacilityId { get; set; }
    
    public Guid? LineId { get; set; }
    public string? ExternalLineName { get; set; }
    public bool ExternalLinePlaced { get; set; }
    public string? ExternalLinePlacedBy { get; set; }
    public DateTime? ExternalLineInsertedOn { get; set; }
    
    public bool IsStat { get; set; }
    public bool IsOnHold { get; set; }
    public string? StatReason { get; set; }
    public string? HoldReason { get; set; }
    
    public Guid? MedicalRecordId { get; set; }
    public string? MedicalRecordNumber { get; set; }
}