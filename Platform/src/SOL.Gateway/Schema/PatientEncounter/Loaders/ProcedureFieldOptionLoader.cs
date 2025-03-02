using Microsoft.EntityFrameworkCore;
using SOL.Gateway.Views.PatientEncounter.Procedure;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldOptionLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, ProcedureFieldOptionView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, ProcedureFieldOptionView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var query = await dbCtx.Set<ProcedureFieldOptionView>()
            .Where(x => keys.Contains(x.FieldId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FieldId);
    }
}
