using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobRoutineLoader : BatchDataLoader<Guid, JobRoutineView>
{
    private readonly IDomainQuery<JobRoutineView> _getRoutine;

    public JobRoutineLoader(IDomainQuery<JobRoutineView> getRoutine
        , IBatchScheduler batchScheduler
        , DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _getRoutine = getRoutine;
    }
    
    protected override async Task<IReadOnlyDictionary<Guid, JobRoutineView>> LoadBatchAsync(
        IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var routines = await _getRoutine.Query
            .Where(x => keys.Contains(x.JobId))
            .ToListAsync(cancellationToken);

        return routines.ToDictionary(x => x.JobId);
    }
}