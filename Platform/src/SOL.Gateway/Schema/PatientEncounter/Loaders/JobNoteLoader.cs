using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Job;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobNoteLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, JobNoteView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, JobNoteView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var notes = await dbCtx.Set<JobNoteView>()
            .Where(x => keys.Contains(x.JobId))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return notes.ToLookup(x => x.JobId);
    }
}