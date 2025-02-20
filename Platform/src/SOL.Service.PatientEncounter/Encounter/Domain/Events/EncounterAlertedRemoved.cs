using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterAlertedRemoved))]
public class EncounterAlertedRemoved:  IMessage
{
    public Guid EncounterId { get; }
    public EncounterAlertType Type { get; }
    
    internal EncounterAlertedRemoved(Guid encounterId, EncounterAlertType type)
    {
        EncounterId = encounterId;
        Type = type;
    }
}