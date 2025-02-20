using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

[TrackableActivity(nameof(EncounterMedicalRecordChanged))]
public record EncounterMedicalRecordChanged : IMessage
{
    public Guid Id { get; init; }
    public Guid? MedicalRecordId { get; init; }
    
    internal EncounterMedicalRecordChanged(Encounter encounter)
    {
        Id = encounter.Id;
        MedicalRecordId = encounter.MedicalRecordId;
    }
}