using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RollingScheduleLoader : BatchDataLoader<Guid, RollingScheduleView>
{
    private readonly IDomainQuery<RollingScheduleView> _getRoutineRollings;

    public RollingScheduleLoader(IDomainQuery<RollingScheduleView> getRoutineRollings, IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoutineRollings = getRoutineRollings;
    }

    protected override async Task<IReadOnlyDictionary<Guid, RollingScheduleView>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var rollings = await _getRoutineRollings.Query.Where(x => keys.Contains(x.RoutineId)).ToListAsync(cancellationToken);
        return rollings.ToDictionary(x => x.RoutineId);
    }
}
