using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Line.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineEncounterLoader : GroupedDataLoader<Guid, Guid>
{
    private readonly IDomainQuery<LineEncounterView> _getJobLine;

    public LineEncounterLoader(IDomainQuery<LineEncounterView> getJobLine, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getJobLine = getJobLine;
    }

    protected override async Task<ILookup<Guid, Guid>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getJobLine.Query
            .Where(x => keys.Contains(x.EncounterId))
            .Select(x => new { JobId = x.EncounterId, x.Id })
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.JobId, y => y.Id);
    }
}