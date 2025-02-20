using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CreateJob : IMessage
{
    public Guid RoomId { get; init; }
    public Guid PurposeId { get; init; }
    public string? Contact { get; init; }
    public string? OrderingProvider { get; init; }
    public string? PreNote { get; init; }
    public LocalDate? ScheduledDate { get; init; }
    public LocalTime? ScheduledTime { get; init; }
    public Guid? LineId { get; init; }
    public Guid? MedicalRecordId { get; init; }
    public Guid? RoutineProcedureId { get; init; }
    public Guid? RoutineAssignmentId { get; init; }
}