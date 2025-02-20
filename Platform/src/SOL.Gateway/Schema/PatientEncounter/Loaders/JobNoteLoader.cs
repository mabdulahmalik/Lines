using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobNoteLoader : GroupedDataLoader<Guid, JobNoteView>
{
    private readonly IDomainQuery<JobNoteView> _getNote;

    public JobNoteLoader(IDomainQuery<JobNoteView> getNote, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getNote = getNote;
    }

    protected override async Task<ILookup<Guid, JobNoteView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var notes = await _getNote.Query
            .Where(x => keys.Contains(x.JobId))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return notes.ToLookup(x => x.JobId);
    }
}