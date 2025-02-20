using SOL.Abstractions.Domain;

namespace SOL.Abstractions.Persistence;

public interface IAggregateRepository<TAggregateRoot> : IAggregateReader<TAggregateRoot>
    where TAggregateRoot : AggregateRoot
{
    Task Add(TAggregateRoot aggregateRoot, CancellationToken stoppageToken = default);
    void Update(TAggregateRoot aggregateRoot);
    void Sort(Guid id, int prevPosition, int curPosition);
    Task Delete(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Restore(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Archive(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Unarchive(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Commit(CancellationToken stoppageToken = default);
}