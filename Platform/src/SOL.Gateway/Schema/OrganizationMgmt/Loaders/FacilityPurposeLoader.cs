using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Facility;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityPurposeLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityPurposeView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityPurposeView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityPurposeView>()
            .Where(x => keys.Contains(x.FacilityId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityId);
    }
}