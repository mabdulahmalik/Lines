using MassTransit;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.EncounterModification;

public class EncounterModificationState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid? MedicalRecordId { get; set; }
}