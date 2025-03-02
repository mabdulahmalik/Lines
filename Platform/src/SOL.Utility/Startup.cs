using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Storage;
using SOL.IdP;
using SOL.IdP.Keycloak;
using SOL.IdP.Phase2;
using SOL.Utility.Caching;
using SOL.Utility.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddCachingSystem(this IServiceCollection services, Action<CachingSystemOptions> setCachingSystemOptions)
    {
        var opts = new CachingSystemOptions();
        setCachingSystemOptions(opts);
        
        services
            .AddTransient<ICacheManager, CacheManager>()
            .AddStackExchangeRedisCache(options => {
                options.Configuration = opts.RedisConnectionString;
            });

        return services;
    }
    
    public static IServiceCollection AddStorageSystem(this IServiceCollection services, Action<StorageSystemOptions> setStorageSystemOptions)
    {
        var opts = new StorageSystemOptions();
        setStorageSystemOptions(opts);        
        
        services.AddScoped<IFileStorage>(sp => {
            var serviceClient = new BlobServiceClient(opts.AzureConnectionString);
            return new AzureFileStorage(serviceClient
                , sp.GetService<IConfiguration>()
                , sp.GetService<IOperationContextFactory>());
        });

        return services;
    }

    public static IServiceCollection AddIdPClients(this IServiceCollection services, Action<IdPClientOptions> setIdPClientOptions)
    {
        var opts = new IdPClientOptions();
        setIdPClientOptions(opts);

        services.AddHttpClient<IKeycloakAdminClient, KeycloakAdminClient>(httpClientConfig);
        services.AddHttpClient<IPhase2AdminClient, Phase2AdminClient>(httpClientConfig);
        services.AddHttpClient<KeycloakAuthenticator>(c => c.BaseAddress = new Uri(opts.BaseUrl));
        
        return services;

        void httpClientConfig(IServiceProvider sp, HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(opts.BaseUrl);
            
            try
            {
                var kcAuthMgr = sp.GetService<KeycloakAuthenticator>();
                var accessToken = kcAuthMgr
                    .GetAccessToken(opts.ClientId, opts.ClientSecret)
                    .Result;
                
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            }
            catch (Exception ex)
            {
                var logger = sp.GetService<ILoggerFactory>();
                logger?.CreateLogger("httpClientConfig")
                    .LogCritical(ex, "Failed to configure HttpClient with Bearer token.");
            }
        }
    }
}

public class CachingSystemOptions
{
    public string? RedisConnectionString { get; set; }
}

public class StorageSystemOptions
{
    public string? AzureConnectionString { get; set; }
}

public class IdPClientOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}