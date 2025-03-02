using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityRoomPropertyView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityRoomPropertyView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityRoomPropertyView>()
            .Where(x => keys.Contains(x.FacilityTypeId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityTypeId);
    }
}
