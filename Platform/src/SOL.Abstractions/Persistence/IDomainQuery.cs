namespace SOL.Abstractions.Persistence;

public interface IDomainQuery<T> where T : class
{
    IQueryable<T> Query { get; }
}