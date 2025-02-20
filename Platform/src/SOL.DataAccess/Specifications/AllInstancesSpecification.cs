using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.DataAccess.Specifications;

public class AllInstancesSpecification<T> : AggregateQuerySpecification<T>
    where T : AggregateRoot
{
    public AllInstancesSpecification() 
        : base(b => true)
    { }
}