using SOL.Abstractions.Domain;

namespace SOL.Abstractions.Persistence;

/// <summary>
/// Defines an interceptor for the commit operation, allowing additional behavior to be injected
/// during the commit process in the repository. 
/// </summary>
public interface ICommitInterceptor
{
    /// <summary>
    /// Executes additional actions after the repository's commit process is initiated. 
    /// This method is called at the end of the repository's Commit method and can be used to 
    /// add any required post-commit behavior.
    /// </summary>
    Task Commit(IEnumerable<AggregateRoot> aggregates, CancellationToken stoppageToken = default);
}