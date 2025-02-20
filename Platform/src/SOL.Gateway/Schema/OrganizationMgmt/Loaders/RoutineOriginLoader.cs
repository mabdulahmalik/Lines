using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineOriginLoader : GroupedDataLoader<Guid, RoutineOriginView>
{
    private readonly IDomainQuery<RoutineOriginView> _getRoutineOriginQuery;

    public RoutineOriginLoader(IDomainQuery<RoutineOriginView> getRoutineOriginQuery,
        IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoutineOriginQuery = getRoutineOriginQuery;
    }

    protected override async Task<ILookup<Guid, RoutineOriginView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoutineOriginQuery.Query
            .Where(x => keys.Contains(x.RoutineId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.RoutineId);
    }
}
