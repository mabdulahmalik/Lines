using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.FacilityRoom.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyLoader : GroupedDataLoader<Guid, FacilityRoomPropertyView>
{
    private readonly IDomainQuery<FacilityRoomPropertyView> _getRoomPropertyQuery;

    public FacilityRoomPropertyLoader(IDomainQuery<FacilityRoomPropertyView> getProcedureFieldQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoomPropertyQuery = getProcedureFieldQuery;
    }

    protected override async Task<ILookup<Guid, FacilityRoomPropertyView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoomPropertyQuery.Query
            .Where(x => keys.Contains(x.FacilityTypeId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FacilityTypeId);
    }
}
