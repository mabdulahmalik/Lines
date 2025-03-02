using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Procedure;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, ProcedureFieldView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, ProcedureFieldView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<ProcedureFieldView>()
            .Where(x => keys.Contains(x.ProcedureId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.ProcedureId);
    }
}