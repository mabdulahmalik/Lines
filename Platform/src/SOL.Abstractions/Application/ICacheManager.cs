namespace SOL.Abstractions.Application;

public interface ICacheManager
{
    Task<string?> Get(string key);
    Task Set(string key, string value, DateTimeOffset? expiration = null);
}