using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, EncounterProcedureView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, EncounterProcedureView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var procedures = await dbCtx.Set<EncounterProcedureView>()
            .Where(x => keys.Contains(x.EncounterId))
            .OrderBy(x => x.PerformedAt)
            .ToListAsync(cancellationToken);

        return procedures.ToLookup(x => x.EncounterId);
    }
}