using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.UserMgmt.User;

namespace SOL.Gateway.Schema.UserMgmt.Loaders;

public class UserStatusLoader : BatchDataLoader<Guid, UserStatusView>
{
    private readonly IDbContextFactory<LinesDataStore> _dbCtxFactory;

    public UserStatusLoader(IDbContextFactory<LinesDataStore> dbCtxFactory
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _dbCtxFactory = dbCtxFactory;
    }

    protected override async Task<IReadOnlyDictionary<Guid, UserStatusView>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await _dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var statuses = await dbCtx.Set<UserStatusView>()
            .Where(x => keys.Contains(x.UserId))
            .ToListAsync(cancellationToken);

        return statuses.ToDictionary(x => x.UserId);
    }
}