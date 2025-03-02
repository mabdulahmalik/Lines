using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Facility;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyValueLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityRoomPropertyValueView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityRoomPropertyValueView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityRoomPropertyValueView>()
            .Where(x => keys.Contains(x.RoomId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.RoomId);
    }
}
