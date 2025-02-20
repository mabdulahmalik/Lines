using System.Linq.Expressions;
using SOL.Abstractions.Domain;

namespace SOL.Abstractions.Persistence;

public interface ISpecification<T>
    where T : AggregateRoot
{
    Expression<Func<T, bool>> Criteria { get; }
    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
}