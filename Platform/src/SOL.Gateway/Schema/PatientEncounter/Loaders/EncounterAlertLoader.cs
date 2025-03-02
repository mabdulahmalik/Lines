using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAlertLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, EncounterAlertView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, EncounterAlertView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var alerts = await dbCtx.Set<EncounterAlertView>()
            .Where(x => keys.Contains(x.EncounterId))
            .OrderByDescending(x => x.AlertedAt)
            .ToListAsync(cancellationToken);

        return alerts.ToLookup(x => x.EncounterId);
    }
}