using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.MedicalRecord.Domain;

public record MedicalRecordObservationsRemoved : IMessage
{
    public Guid MedicalRecordId { get; }
    public IReadOnlyList<Observation> Observations { get; }
    
    public MedicalRecordObservationsRemoved(Guid medicalRecordId, IEnumerable<Observation> observations)
    {
        MedicalRecordId = medicalRecordId;
        Observations = observations.ToList();
    }
}