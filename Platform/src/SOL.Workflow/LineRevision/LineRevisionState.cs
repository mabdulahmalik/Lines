using MassTransit;
using NodaTime;

namespace SOL.Workflow.LineRevision;

public class LineRevisionState : SagaStateMachineInstance, ISagaVersion
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    
    public Guid LineId { get; set; }
    public string? LineName { get; set; }
    
    public Guid FacilityId { get; set; }
    public Guid RoomId { get; set; }
    public string? RoomName { get; set; }
    
    public Guid? MedicalRecordId { get; set; }
    public string? Number { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public LocalDate? Birthday { get; set; }
}