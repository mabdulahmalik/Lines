using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EnactLineRevision : IMessage
{
    public Guid Id { get; init; }
    public string? Name { get; init; }    
    
    public IntakeLocation Location { get; init; }
    public ModifyMedicalRecord MedicalRecord { get; init; }    
}