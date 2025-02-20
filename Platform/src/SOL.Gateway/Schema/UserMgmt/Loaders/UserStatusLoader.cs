using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.User.View;

namespace SOL.Gateway.Schema.UserMgmt.Loaders;

public class UserStatusLoader : BatchDataLoader<Guid, UserStatusView>
{
    private readonly IDomainQuery<UserStatusView> _userStatusQuery;

    public UserStatusLoader(IDomainQuery<UserStatusView> userStatusQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _userStatusQuery = userStatusQuery;
    }

    protected override async Task<IReadOnlyDictionary<Guid, UserStatusView>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var statuses = await _userStatusQuery.Query
            .Where(x => keys.Contains(x.UserId))
            .ToListAsync();

        return statuses.ToDictionary(x => x.UserId);
    }
}