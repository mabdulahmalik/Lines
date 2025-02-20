using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoutineSpecLoader : GroupedDataLoader<Guid, FacilityRoutineSpecView>
{
    private readonly IDomainQuery<FacilityRoutineSpecView> _getAssignmentSpecQuery;

    public FacilityRoutineSpecLoader(IDomainQuery<FacilityRoutineSpecView> getProcedureFieldQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getAssignmentSpecQuery = getProcedureFieldQuery;
    }

    protected override async Task<ILookup<Guid, FacilityRoutineSpecView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getAssignmentSpecQuery.Query
            .Where(x => keys.Contains(x.FacilityRoutineId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityRoutineId);
    }
}