using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityPurposeLoader : GroupedDataLoader<Guid, FacilityPurposeView>
{
    private readonly IDomainQuery<FacilityPurposeView> _getFacilityPurpose;

    public FacilityPurposeLoader(IDomainQuery<FacilityPurposeView> getFacilityPurpose, IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getFacilityPurpose = getFacilityPurpose;
    }

    protected override async Task<ILookup<Guid, FacilityPurposeView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getFacilityPurpose.Query
            .Where(x => keys.Contains(x.FacilityId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityId);
    }
}