using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyValueLoader : GroupedDataLoader<Guid, FacilityRoomPropertyValueView>
{
    private readonly IDomainQuery<FacilityRoomPropertyValueView> _getRoomPropertiesQuery;

    public FacilityRoomPropertyValueLoader(IDomainQuery<FacilityRoomPropertyValueView> getJobLine, IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _getRoomPropertiesQuery = getJobLine;
    }

    protected override async Task<ILookup<Guid, FacilityRoomPropertyValueView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getRoomPropertiesQuery.Query
            .Where(x => keys.Contains(x.RoomId))
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.RoomId);
    }
}
