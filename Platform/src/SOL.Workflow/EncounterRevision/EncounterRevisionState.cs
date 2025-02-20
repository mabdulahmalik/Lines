using MassTransit;
using NodaTime;

namespace SOL.Workflow.EncounterRevision;

public class EncounterRevisionState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    
    public Guid JobId { get; set; }
    public Guid? EncounterId { get; set; }
    
    public string? OrderingProvider { get; set; }
    public string? Contact { get; set; }
    
    public Guid FacilityId { get; set; }
    public Guid RoomId { get; set; }
    public string? RoomName { get; set; }
    
    public Guid? MedicalRecordId { get; set; }
    public string? Number { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public LocalDate? Birthday { get; set; }
}