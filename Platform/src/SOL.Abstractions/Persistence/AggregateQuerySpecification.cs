using System.Linq.Expressions;
using SOL.Abstractions.Domain;

namespace SOL.Abstractions.Persistence;

public abstract class AggregateQuerySpecification<T> : ISpecification<T>
    where T : AggregateRoot
{
    private readonly List<Expression<Func<T, object>>> _includes = new();
    
    public AggregateQuerySpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
    
    public Expression<Func<T, bool>> Criteria { get; }

    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }
}