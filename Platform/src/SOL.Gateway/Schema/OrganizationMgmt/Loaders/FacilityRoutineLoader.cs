using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoutineLoader : GroupedDataLoader<Guid, FacilityRoutineView>
{
    private readonly IDomainQuery<FacilityRoutineView> _getRoutineAssignmentQuery;

    public FacilityRoutineLoader(IDomainQuery<FacilityRoutineView> getProcedureFieldQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoutineAssignmentQuery = getProcedureFieldQuery;
    }

    protected override async Task<ILookup<Guid, FacilityRoutineView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoutineAssignmentQuery.Query
            .Where(x => keys.Contains(x.FacilityId))
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityId);
    }
}
