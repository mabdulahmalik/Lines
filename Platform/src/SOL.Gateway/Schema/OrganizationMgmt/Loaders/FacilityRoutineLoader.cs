using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Facility;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoutineLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityRoutineView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityRoutineView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityRoutineView>()
            .Where(x => keys.Contains(x.FacilityId))
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityId);
    }
}
