using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Dawn;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;

namespace SOL.DataAccess;

public class GenericRepository<TDbCtx, TAggregateRoot> : IAggregateRepository<TAggregateRoot>
    where TAggregateRoot : AggregateRoot
    where TDbCtx : DbContext
{
    private readonly Tracked<IMessage> _domainEvents = new();
    private readonly StringBuilder _rawSql = new();
    
    private readonly Lazy<TDbCtx> _dbCtx;
    private readonly IDomainBus _domainBus;
    private readonly IClock _clock;

    private readonly IEnumerable<ICommitInterceptor> _interceptors;

    public GenericRepository(IDbContextFactory<TDbCtx> dbContextFactory, IDomainBus domainBus, IClock clock, IEnumerable<ICommitInterceptor> interceptors)
    {
        _clock = clock;
        _domainBus = domainBus;
        _dbCtx = new(dbContextFactory.CreateDbContext);
        _interceptors = interceptors;
    }

    public async Task<TAggregateRoot> Get(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        var queryableResultWithIncludes = spec.Includes
            .Aggregate(_dbCtx.Value.Set<TAggregateRoot>().AsQueryable(),
                (current, include) => current.Include(include));        
        
        return await queryableResultWithIncludes.SingleAsync(spec.Criteria, stoppageToken);
    }
    
    public async Task<TAggregateRoot> Get(Guid id, CancellationToken stoppageToken = default)
    {
        return await Get(new SingleInstanceSpecification<TAggregateRoot>(id), stoppageToken);
    }    

    public async Task<bool> Exists(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        return await _dbCtx.Value.Set<TAggregateRoot>().AnyAsync(spec.Criteria, stoppageToken);
    }

    public async Task<bool> Exists(Guid id, CancellationToken stoppageToken = default)
    {
        return await Exists(new SingleInstanceSpecification<TAggregateRoot>(id), stoppageToken);
    }
    
    public async Task<IReadOnlyList<TAggregateRoot>> List(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        var queryableResultWithIncludes = spec.Includes
            .Aggregate(_dbCtx.Value.Set<TAggregateRoot>().AsQueryable(),
                (current, include) => current.Include(include));
        
        if (typeof(TAggregateRoot).IsAssignableTo(typeof(ISortable)))
            queryableResultWithIncludes = queryableResultWithIncludes
                .OrderBy(x => EF.Property<int>(x, nameof(ISortable.Order)));
        
        return await queryableResultWithIncludes.Where(spec.Criteria).ToListAsync(stoppageToken);
    }

    public async Task Add(TAggregateRoot aggregateRoot, CancellationToken stoppageToken = default)
    {
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var entry = dbSet.Entry(aggregateRoot);
        
        await dbSet.AddAsync(aggregateRoot, stoppageToken);
        entry.Property(x => x.CreatedAt).CurrentValue = _clock.GetCurrentInstant();
    }

    public Task Update(TAggregateRoot aggregateRoot, CancellationToken stoppageToken = default)
    {
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var entry = dbSet.Entry(aggregateRoot);
        
        if(entry.State == EntityState.Detached) {
            dbSet.Attach(aggregateRoot);
        }        
        
        entry.Property(x => x.ModifiedAt).CurrentValue = _clock.GetCurrentInstant();
        
        return Task.CompletedTask;
    }

    public Task Sort(Guid id, int prevPosition, int curPosition, CancellationToken stoppageToken = default)
    {
        Guard.Support(typeof(TAggregateRoot).IsAssignableTo(typeof(ISortable))
            , $"Cannot sort any [{typeof(TAggregateRoot).Name}] objects because it does not implement {typeof(ISortable).FullName}.");
        
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var schemaName = dbSet.EntityType.GetSchema();
        var tableName = dbSet.EntityType.GetTableName();

        _rawSql.Append("EXEC [dbo].[spSortEntityItems] ");
        _rawSql.AppendFormat("@entityTable = '[{0}].[{1}]', @entityId = '{2}', @currPos = {3}, @prevPos = {4};"
            , schemaName, tableName, id, curPosition, prevPosition);
        _rawSql.AppendLine();
        
        _domainEvents.Add(new AggregateSorted { Id = id, Name = typeof(TAggregateRoot).Name, PosOld = prevPosition, PosNew = curPosition });
        
        return Task.CompletedTask;
    }

    public async Task Delete(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var results = await List(spec, stoppageToken);
        var aggregateRootIds = results.Select(x => x.Id).ToList();
        
        if (typeof(TAggregateRoot).IsAssignableTo(typeof(IRestorable)))
        {
            var schemaName = dbSet.EntityType.GetSchema();
            var tableName = dbSet.EntityType.GetTableName();
            var columName = dbSet.EntityType.FindProperty(nameof(IRestorable.DeletedAt))?.GetColumnName();
            
            _rawSql.AppendFormat("UPDATE [{0}].[{1}] SET [{2}] = SYSDATETIME() WHERE [Id] IN('{3}');"
                , schemaName, tableName, columName, string.Join("','", aggregateRootIds));
            _rawSql.AppendLine();
        }
        else
        {
            dbSet.RemoveRange(results);
        }
        
        if(aggregateRootIds.Any())
            _domainEvents.Add(new AggregateDeleted { Ids = aggregateRootIds, Name = typeof(TAggregateRoot).Name });
    }

    public async Task Restore(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        Guard.Support(typeof(TAggregateRoot).IsAssignableTo(typeof(IRestorable))
            , $"Cannot restore any [{typeof(TAggregateRoot).Name}] objects because it does not implement {typeof(IRestorable).FullName}.");        
        
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var schemaName = dbSet.EntityType.GetSchema();
        var tableName = dbSet.EntityType.GetTableName();
        var columName = dbSet.EntityType.FindProperty(nameof(IRestorable.DeletedAt))?.GetColumnName();
        
        var results = await List(spec, stoppageToken);
        var aggregateRootIds = results.Select(x => x.Id).ToList();
        
        _rawSql.AppendFormat("UPDATE [{0}].[{1}] SET [{2}] = NULL WHERE [Id] IN('{3}');"
            , schemaName, tableName, columName, string.Join("','", aggregateRootIds));
        _rawSql.AppendLine();
        
        _domainEvents.Add(new AggregateRestored { Ids = aggregateRootIds, Name = typeof(TAggregateRoot).Name });
    }
    
    public async Task Archive(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        Guard.Support(typeof(TAggregateRoot).IsAssignableTo(typeof(IArchivable))
            , $"Cannot Archive any [{typeof(TAggregateRoot).Name}] objects because it does not implement {typeof(IArchivable).FullName}.");        
        
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var schemaName = dbSet.EntityType.GetSchema();
        var tableName = dbSet.EntityType.GetTableName();
        var columName = dbSet.EntityType.FindProperty(nameof(IArchivable.Archived))?.GetColumnName();
        
        var results = await List(spec, stoppageToken);
        var aggregateRootIds = results.Select(x => x.Id).ToList();
        
        _rawSql.AppendFormat("UPDATE [{0}].[{1}] SET [{2}] = 1 WHERE [Id] IN('{3}');"
            , schemaName, tableName, columName, string.Join("','", aggregateRootIds));
        _rawSql.AppendLine();
        
        _domainEvents.Add(new AggregateArchiveStateChanged { Ids = aggregateRootIds, Name = typeof(TAggregateRoot).Name, Archived = true });
    }
    
    public async Task Unarchive(ISpecification<TAggregateRoot> spec, CancellationToken stoppageToken = default)
    {
        Guard.Support(typeof(TAggregateRoot).IsAssignableTo(typeof(IArchivable))
            , $"Cannot Unarchive any [{typeof(TAggregateRoot).Name}] objects because it does not implement {typeof(IArchivable).FullName}.");        
        
        var dbSet = _dbCtx.Value.Set<TAggregateRoot>();
        var schemaName = dbSet.EntityType.GetSchema();
        var tableName = dbSet.EntityType.GetTableName();
        var columName = dbSet.EntityType.FindProperty(nameof(IArchivable.Archived))?.GetColumnName();
        
        var results = await List(spec, stoppageToken);
        var aggregateRootIds = results.Select(x => x.Id).ToList();
        
        _rawSql.AppendFormat("UPDATE [{0}].[{1}] SET [{2}] = 0 WHERE [Id] IN('{3}');"
            , schemaName, tableName, columName, string.Join("','", aggregateRootIds));
        _rawSql.AppendLine();
        
        _domainEvents.Add(new AggregateArchiveStateChanged { Ids = aggregateRootIds, Name = typeof(TAggregateRoot).Name, Archived = false });
    }    

    public async Task Commit(CancellationToken stoppageToken = default)
    {
        if (!_dbCtx.IsValueCreated)
            return;
        
        var domainEvents = _dbCtx.Value.ChangeTracker.Entries()
            .Select(e => e.Entity).OfType<TAggregateRoot>()
            .SelectMany(x => x.Changes).Union(_domainEvents)
            .OrderBy(x => x.TimeStamp)
            .ToList();

        if (domainEvents.Any())
        {
            await _dbCtx.Value.SaveChangesAsync(stoppageToken);

            if (_rawSql.Length > 0)
                await _dbCtx.Value.Database.ExecuteSqlRawAsync(_rawSql.ToString(), stoppageToken);

            foreach (var change in domainEvents.Select(x => x.Value))
                await _domainBus.DispatchAsync(change, stoppageToken);

            var aggregateRoots = _dbCtx.Value.ChangeTracker.Entries()
                                    .Select(e => e.Entity)
                                    .OfType<TAggregateRoot>()
                                    .ToList();

            foreach (var interceptor in _interceptors)
                await interceptor.Commit(aggregateRoots, stoppageToken);
        }
    }

    public void Dispose()
    {
        if (_dbCtx.IsValueCreated)
        {
            _dbCtx.Value.Dispose();
        }
    }
}