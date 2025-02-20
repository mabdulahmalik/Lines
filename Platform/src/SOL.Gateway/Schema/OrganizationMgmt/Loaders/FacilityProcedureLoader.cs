using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityProcedureLoader : GroupedDataLoader<Guid, FacilityProcedureView>
{
    private readonly IDomainQuery<FacilityProcedureView> _getFacilityProcedure;

    public FacilityProcedureLoader(IDomainQuery<FacilityProcedureView> getJobLine, IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getFacilityProcedure = getJobLine;
    }

    protected override async Task<ILookup<Guid, FacilityProcedureView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getFacilityProcedure.Query
            .Where(x => keys.Contains(x.FacilityId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityId);
    }
}
