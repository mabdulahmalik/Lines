using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record CreateRoutineFollowUp : IMessage
{
    public Guid EncounterProcedureId { get; init; }
    public Guid RoutineAssignmentId { get; init; }
    public Guid FacilityRoomId { get; init; }
    public Guid? MedicalRecordId { get; init; }
    public Guid? LineId { get; init; }
    public bool InitialRun { get; init; } = false;
}