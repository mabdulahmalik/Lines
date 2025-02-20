using SOL.Abstractions.Persistence;

namespace SOL.Service.PatientEncounter.Line.Domain;

public class SingleLineByEncounterSpecification : AggregateQuerySpecification<Line>
{
    public SingleLineByEncounterSpecification(Guid encounterId) 
        : base(x => x.EncounterIds.Any(y => y.Value == encounterId))
    { }
}