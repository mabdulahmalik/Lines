using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.FacilityRoom.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyOptionLoader : GroupedDataLoader<Guid, FacilityRoomPropertyOptionView>
{
    private readonly IDomainQuery<FacilityRoomPropertyOptionView> _getRoomPropertyOptionQuery;

    public FacilityRoomPropertyOptionLoader(IDomainQuery<FacilityRoomPropertyOptionView> getProcedureFieldQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoomPropertyOptionQuery = getProcedureFieldQuery;
    }

    protected override async Task<ILookup<Guid, FacilityRoomPropertyOptionView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoomPropertyOptionQuery.Query
            .Where(x => keys.Contains(x.PropertyId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.PropertyId);
    }
}