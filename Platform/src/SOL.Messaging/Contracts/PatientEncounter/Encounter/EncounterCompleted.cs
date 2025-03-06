using System.Reflection.Emit;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterCompleted : IMessage
{
    public Guid Id { get; init; }
    public Guid CompletedBy { get; init; }
    public string Purpose { get; init; }
    public IEnumerable<PatientAttendee> Attendees { get; init; }
}