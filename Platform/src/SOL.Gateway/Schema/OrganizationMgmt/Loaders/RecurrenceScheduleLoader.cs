using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RecurrenceScheduleLoader : GroupedDataLoader<Guid, RecurrenceScheduleView>
{
    private readonly IDomainQuery<RecurrenceScheduleView> _getRoutineRecurrenceQuery;

    public RecurrenceScheduleLoader(IDomainQuery<RecurrenceScheduleView> getRoutineRecurrenceQuery,
        IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoutineRecurrenceQuery = getRoutineRecurrenceQuery;
    }

    protected override async Task<ILookup<Guid, RecurrenceScheduleView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoutineRecurrenceQuery.Query
            .Where(x => keys.Contains(x.RoutineId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.RoutineId);
    }
}
