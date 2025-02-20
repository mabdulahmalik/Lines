using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterAlertAdded : IMessage
{
    public Guid EncounterId { get; init; }
    public EncounterAlertType Type { get; init; }
    public Instant AlertedAt { get; init; }
    public Guid AlertedBy { get; set; }
    public string? Text { get; init; }    
}