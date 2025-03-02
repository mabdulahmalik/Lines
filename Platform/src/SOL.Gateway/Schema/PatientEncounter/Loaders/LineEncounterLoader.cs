using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Line;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineEncounterLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, Guid>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, Guid>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<LineEncounterView>()
            .Where(x => keys.Contains(x.EncounterId))
            .Select(x => new { JobId = x.EncounterId, x.Id })
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.JobId, y => y.Id);
    }
}