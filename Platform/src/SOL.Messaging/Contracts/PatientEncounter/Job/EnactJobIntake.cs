using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EnactJobIntake : IMessage
{
    public Guid PurposeId { get; init; }
    public string? Contact { get; init; }
    public string? PreNote { get; init; }
    public string? OrderingProvider { get; init; }
    
    public IntakeLine Line { get; init; }
    public IntakeSchedule Schedule { get; init; }
    public IntakeMedicalRecord MedicalRecord { get; init; }
    public IntakeUrgency Urgency { get; init; }
    public IntakeLocation Location { get; init; }
}

public record IntakeLocation
{
    public Guid FacilityId { get; init; }
    public Guid? RoomId { get; init; }
    public string? RoomName { get; init; }
}