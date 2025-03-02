using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureValueLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, EncounterProcedureValueView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, EncounterProcedureValueView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var procedureValues = await dbCtx.Set<EncounterProcedureValueView>()
            .Where(x => keys.Contains(x.EncounterProcedureId))
            .ToListAsync(cancellationToken);

        return procedureValues.ToLookup(x => x.EncounterProcedureId);
    }
}