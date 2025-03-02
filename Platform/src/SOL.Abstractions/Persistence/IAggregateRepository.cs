using SOL.Abstractions.Domain;

namespace SOL.Abstractions.Persistence;

public interface IAggregateRepository<TAggregateRoot> : IAggregateReader<TAggregateRoot>
    where TAggregateRoot : AggregateRoot
{
    Task Add(TAggregateRoot aggregateRoot, CancellationToken stoppageToken = default);
    Task Update(TAggregateRoot aggregateRoot, CancellationToken stoppageToken = default);
    Task Sort(Guid id, int prevPosition, int curPosition, CancellationToken stoppageToken = default);
    Task Delete(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Restore(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Archive(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Unarchive(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task Commit(CancellationToken stoppageToken = default);
}