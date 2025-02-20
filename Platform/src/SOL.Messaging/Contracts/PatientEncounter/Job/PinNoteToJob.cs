using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record PinNoteToJob : IMessage
{
    public Guid Id { get; init; }
    public Guid JobId { get; init; }
}