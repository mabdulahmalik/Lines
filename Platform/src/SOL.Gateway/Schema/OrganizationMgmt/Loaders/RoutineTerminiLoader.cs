using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutineTerminiLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, RoutineTerminiView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, RoutineTerminiView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<RoutineTerminiView>()
            .Where(x => keys.Contains(x.RoutineId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.RoutineId);
    }
}
