using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ModifyEncounter : IMessage
{
    public Guid? Id { get; init; }
    public Guid JobId { get; init; }
    public Guid RoomId { get; init; }
    public Guid FacilityId { get; init; }
    
    public string? Contact { get; init; }
    public string? OrderingProvider { get; init; }    
    public ModifyMedicalRecord MedicalRecord { get; init; }
}