using MassTransit;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Workflows.LineModification;

public class LineModificationState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid? MedicalRecordId { get; set; }    
}