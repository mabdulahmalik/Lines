using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record LineMedicalRecordModified : IMessage
{
    public Guid LineId { get; init; }
    public Guid? MedicalRecordId { get; init; }
    public Guid? OldMedicalRecordId { get; init; }
}
