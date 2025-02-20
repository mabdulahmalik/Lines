using Microsoft.Extensions.Caching.Distributed;
using SOL.Abstractions.Application;

namespace SOL.Utility.Caching;

public class CacheManager : ICacheManager
{
    private readonly IDistributedCache _cache;
    private readonly Lazy<IOperationContext> _operationContext;
    
    public CacheManager(IDistributedCache cache, IOperationContextFactory operationContextFactory)
    {
        _cache = cache;
        _operationContext = new(operationContextFactory.Get);
    }
    
    public async Task<string?> Get(string key)
    {
        var cacheKey = $"Tenants:{_operationContext.Value.TenantKey}:{key}";
        return await _cache.GetStringAsync(cacheKey.ToLower());
    }
    
    public async Task Set(string key, string value, DateTimeOffset? expiration = null)
    {
        var cacheKey = $"Tenants:{_operationContext.Value.TenantKey}:{key}";
        await _cache.SetStringAsync(cacheKey.ToLower(), value, new DistributedCacheEntryOptions {
            AbsoluteExpiration = expiration
        });
    }
}