using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineMedicalRecordModified))]
public record LineMedicalRecordModified : IMessage
{
    internal LineMedicalRecordModified(Guid id, Guid? medicalRecordId, Guid? oldMedicalRecordId)
    {
        LineId = id;
        MedicalRecordId = medicalRecordId;
        OldMedicalRecordId = oldMedicalRecordId;
    }

    public Guid LineId { get; }
    public Guid? MedicalRecordId { get; }
    public Guid? OldMedicalRecordId { get; }
}