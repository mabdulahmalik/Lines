using MassTransit;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobActivation;

public class JobActivationState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }

    public List<JobEncounterData> EncounterData { get; set; } = new();
}

public class JobEncounterData
{
    public int Order { get; set; }
    public Guid JobId { get; set; }
    public Guid PurposeId { get; set; }
    public Guid FacilityRoomId { get; set; }
    public Guid? MedicalRecordId { get; set; }
}