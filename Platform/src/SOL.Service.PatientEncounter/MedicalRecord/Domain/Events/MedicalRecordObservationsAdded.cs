using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.MedicalRecord.Domain;

public record MedicalRecordObservationsAdded : IMessage
{
    public Guid MedicalRecordId { get; }
    public IReadOnlyList<Observation> Observations { get; }
    
    public MedicalRecordObservationsAdded(Guid medicalRecordId, IEnumerable<Observation> observations)
    {
        MedicalRecordId = medicalRecordId;
        Observations = observations.ToList();
    }
}