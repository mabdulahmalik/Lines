using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.User.View;

namespace SOL.Gateway.Schema.UserMgmt.Loaders;

public class LoggedInUserLoader : BatchDataLoader<Guid, UserView>
{
    private readonly IDomainQuery<UserView> _getUsers;

    public LoggedInUserLoader(IDomainQuery<UserView> getUsers
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _getUsers = getUsers;
    }

    protected override Task<IReadOnlyDictionary<Guid, UserView>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var user = _getUsers.Query
            .Where(x => keys.Contains(x.Id))
            .ToDictionary(x => x.Id);

        return Task.FromResult<IReadOnlyDictionary<Guid, UserView>>(user);
    }
}