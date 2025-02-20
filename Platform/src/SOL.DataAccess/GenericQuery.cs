using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.DataAccess;

public class GenericQuery<TDbCtx, TView> : IDomainQuery<TView>
    where TView : class
    where TDbCtx : DbContext
{
    private readonly Lazy<TDbCtx> _dbCtx;

    public GenericQuery(IDbContextFactory<TDbCtx> dbContextFactory)
    {
        _dbCtx = new(dbContextFactory.CreateDbContext);
    }

    public IQueryable<TView> Query => _dbCtx.Value.Set<TView>().AsNoTracking();
}