using SOL.Abstractions.Persistence;

namespace SOL.Service.PatientEncounter.Line.Domain;

public class MultipleLinesSpecification : AggregateQuerySpecification<Line>
{
    public MultipleLinesSpecification(IEnumerable<Guid> lineIds) 
        : base(x => lineIds.Contains(x.Id))
    { }
}