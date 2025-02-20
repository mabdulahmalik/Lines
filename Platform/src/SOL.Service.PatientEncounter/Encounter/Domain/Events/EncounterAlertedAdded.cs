using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterAlertedAdded))]
public class EncounterAlertedAdded : IMessage
{
    public Guid EncounterId { get; }
    public Guid JobId { get; }
    public Alert Alert { get; }
    
    internal EncounterAlertedAdded(Guid encounterId, Guid jobId, Alert alert)
    {
        EncounterId = encounterId;
        JobId = jobId;
        Alert = alert;
    }
}