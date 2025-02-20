using System.Linq.Expressions;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.DataAccess.Specifications;

public class SingleInstanceSpecification<T> : AggregateQuerySpecification<T>
    where T : AggregateRoot
{
    public SingleInstanceSpecification(Guid id) 
        : base(a => a.Id == id)
    { }
}