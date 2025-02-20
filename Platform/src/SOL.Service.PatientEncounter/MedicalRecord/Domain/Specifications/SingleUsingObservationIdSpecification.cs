using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.Service.PatientEncounter.MedicalRecord.Domain;

public class SingleUsingObservationIdSpecification : AggregateQuerySpecification<MedicalRecord>
{
    public SingleUsingObservationIdSpecification(Guid objectId, ObservationType type) 
        : base(a => a.Observations.Any(o => o.ObjectId == objectId && o.Type == type))
    { }
}