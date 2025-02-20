using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.DataAccess.Specifications;

public class SpecificInstancesSpecification<T> : AggregateQuerySpecification<T>
    where T : AggregateRoot
{
    public SpecificInstancesSpecification(IEnumerable<Guid> aggregateIds)
        : base(a => aggregateIds.Contains(a.Id))
    { }
}