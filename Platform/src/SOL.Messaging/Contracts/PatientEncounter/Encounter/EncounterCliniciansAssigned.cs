using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterCliniciansAssigned : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<EncounterAssignment> Assignments { get; init; }
}

public record EncounterAssignment
{
    public Guid ClinicianId { get; init; }
    public Instant AssignedAt { get; init; }
    public EncounterAssigneePosition Position { get; init; }
}