using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyOptionLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityRoomPropertyOptionView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityRoomPropertyOptionView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityRoomPropertyOptionView>()
            .Where(x => keys.Contains(x.PropertyId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.PropertyId);
    }
}