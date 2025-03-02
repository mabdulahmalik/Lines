using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.OrganizationMgmt.Facility;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityProcedureLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, FacilityProcedureView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, FacilityProcedureView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<FacilityProcedureView>()
            .Where(x => keys.Contains(x.FacilityId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityId);
    }
}
