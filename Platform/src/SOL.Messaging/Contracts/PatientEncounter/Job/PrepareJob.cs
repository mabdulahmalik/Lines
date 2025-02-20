using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record PrepareJob : IMessage
{
    public Guid RoomId { get; init; }
    public Guid FacilityId { get; init; }
    public Guid PurposeId { get; init; }
    public string? Contact { get; init; }
    public string? PreNote { get; init; }
    public string? OrderingProvider { get; init; }
    
    public IntakeMedicalRecord MedicalRecord { get; init; }
    public IntakeSchedule Schedule { get; init; }
    public IntakeUrgency Urgency { get; init; }
    public IntakeLine Line { get; init; }
}

public record IntakeMedicalRecord
{
    public Guid? Id { get; init; }
    public string? Number { get; init; }
}

public record IntakeSchedule
{
    public LocalDate Date { get; init; }
    public LocalTime? Time { get; init; }
}

public record IntakeLine
{
    public bool IsExternallyPlaced { get; init; }
    public Instant? InsertedOn { get; init; }
    public string? PlacedBy { get; init; }
    public string? Name { get; init; }
    public Guid? LineId { get; init; }
}

public record IntakeUrgency
{
    public bool IsStat { get; init; }
    public bool IsOnHold { get; init; }
    public string? StatReason { get; init; }
    public string? HoldReason { get; init; }
}