using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.MedicalRecord.Domain;

[TrackableActivity(nameof(MedicalRecordRenumbered))]
public record MedicalRecordRenumbered : IMessage
{
    public Guid MedicalRecordId { get; }
    public string Number { get; }

    public MedicalRecordRenumbered(Guid medicalRecordId, string number)
    {
        MedicalRecordId = medicalRecordId;
        Number = number;
    }
}