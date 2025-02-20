using MassTransit;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter;

public class PatientEncounterState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Dictionary<Guid, InsertedLine> LinesInserted { get; set; } = new();
    public Dictionary<Guid, Guid> LinesRemoved { get; set; } = new();
    public List<Tuple<Guid, Guid?, bool>> RoutinesAssigned { get; set; } = new();
    public List<Tuple<Guid, Guid>> RoutinesRemoved { get; set; } = new();
    
    public Guid FacilityRoomId { get; set; }
    public Guid? MedicalRecordId { get; set; }
    
}

public class InsertedLine
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime InsertedOn { get; set; }
}