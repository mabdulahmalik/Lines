using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Facility;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoutineSpecLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityRoutineSpecView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityRoutineSpecView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityRoutineSpecView>()
            .Where(x => keys.Contains(x.FacilityRoutineId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityRoutineId);
    }
}