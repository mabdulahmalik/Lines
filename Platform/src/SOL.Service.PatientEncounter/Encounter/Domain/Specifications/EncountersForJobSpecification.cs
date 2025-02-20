using SOL.Abstractions.Persistence;

namespace SOL.Service.PatientEncounter.Encounter.Domain.Specifications;

public class EncountersForJobSpecification : AggregateQuerySpecification<Encounter>
{
    public EncountersForJobSpecification(Guid jobId)
        : base(x => x.JobId == jobId)
    { }
}