using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Job;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobRoutineLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : BatchDataLoader<Guid, JobRoutineView>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, JobRoutineView>> LoadBatchAsync(
        IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var routines = await dbCtx.Set<JobRoutineView>()
            .Where(x => keys.Contains(x.JobId))
            .ToListAsync(cancellationToken);

        return routines.ToDictionary(x => x.JobId);
    }
}