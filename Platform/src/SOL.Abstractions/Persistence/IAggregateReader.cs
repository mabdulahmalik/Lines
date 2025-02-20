using SOL.Abstractions.Domain;

namespace SOL.Abstractions.Persistence;

public interface IAggregateReader<TAggregateRoot> : IDisposable
    where TAggregateRoot : AggregateRoot
{
    Task<TAggregateRoot> Get(Guid id, CancellationToken stoppageToken = default);
    Task<TAggregateRoot> Get(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task<bool> Exists(Guid id, CancellationToken stoppageToken = default);
    Task<bool> Exists(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
    Task<IReadOnlyList<TAggregateRoot>> List(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default);
}