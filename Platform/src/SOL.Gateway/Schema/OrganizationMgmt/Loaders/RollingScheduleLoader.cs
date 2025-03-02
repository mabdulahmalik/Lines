using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RollingScheduleLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : BatchDataLoader<Guid, RollingScheduleView>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, RollingScheduleView>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var rollings = await dbCtx.Set<RollingScheduleView>()
            .Where(x => keys.Contains(x.RoutineId))
            .ToListAsync(cancellationToken);
        
        return rollings.ToDictionary(x => x.RoutineId);
    }
}
