using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ModifyLine : IMessage
{
    public Guid Id { get; init; }
    public Guid RoomId { get; init; }
    public Guid FacilityId { get; init; }
    
    public string? Name { get; init; }

    public ModifyMedicalRecord MedicalRecord { get; init; }    
}