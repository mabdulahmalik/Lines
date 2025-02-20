using MassTransit;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.AttachNoteToJob;

public class AttachNoteToJobState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Note Note { get; set; }
}