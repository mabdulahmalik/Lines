using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.User.View;

namespace SOL.Gateway.Schema.UserMgmt.Loaders;

public class UserRoleLoader : GroupedDataLoader<Guid, UserRoleView>
{
    private readonly IDomainQuery<UserRoleView> _userRoleQuery;

    public UserRoleLoader(IDomainQuery<UserRoleView> userStatusQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _userRoleQuery = userStatusQuery;
    }

    protected override async Task<ILookup<Guid, UserRoleView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var roles = _userRoleQuery.Query
            .Where(x => keys.Contains(x.UserId))
            .ToList();

        return roles.ToLookup(x => x.UserId);
    }
}