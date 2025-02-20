using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineTerminiLoader : GroupedDataLoader<Guid, RoutineTerminiView>
{
    private readonly IDomainQuery<RoutineTerminiView> _getRoutineTerminiQuery;

    public RoutineTerminiLoader(IDomainQuery<RoutineTerminiView> getRoutineTerminiQuery,
        IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoutineTerminiQuery = getRoutineTerminiQuery;
    }

    protected override async Task<ILookup<Guid, RoutineTerminiView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoutineTerminiQuery.Query
            .Where(x => keys.Contains(x.RoutineId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.RoutineId);
    }
}
