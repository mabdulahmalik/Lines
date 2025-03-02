using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAssignmentLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, EncounterAssignmentView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, EncounterAssignmentView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var assignments = await dbCtx.Set<EncounterAssignmentView>()
            .Where(x => keys.Contains(x.EncounterId))
            .OrderBy(x => x.AssignedAt)
            .ToListAsync(cancellationToken);

        return assignments.ToLookup(x => x.EncounterId);
    }
}