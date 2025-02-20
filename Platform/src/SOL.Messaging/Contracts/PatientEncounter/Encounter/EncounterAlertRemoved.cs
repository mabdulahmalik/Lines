using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterAlertRemoved : IMessage
{
    public Guid EncounterId { get; init; }
    public EncounterAlertType Type { get; init; }    
}