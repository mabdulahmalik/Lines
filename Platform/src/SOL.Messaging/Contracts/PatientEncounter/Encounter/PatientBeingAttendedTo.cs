using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record PatientBeingAttendedTo : IMessage
{
    public Guid EncounterId { get; init; }
    public string Purpose { get; init; }
    public IEnumerable<PatientAttendee> Attendees { get; init; }
}

public record PatientAttendee
{
    public Guid ClinicianId { get; init; }
    public EncounterAssigneePosition Position { get; init; }
}