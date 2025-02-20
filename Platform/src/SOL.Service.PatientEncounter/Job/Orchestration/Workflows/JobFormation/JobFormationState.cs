using MassTransit;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation;

public class JobFormationState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid JobId { get; set; }
    public Guid EncounterId { get; set; }
    public Guid? MedicalRecordId { get; set; }
    public Guid? LineId { get; set; }
}