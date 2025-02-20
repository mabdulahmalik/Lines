using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using SOL.Abstractions.Application;
using SOL.Abstractions.Storage;
using SOL.Utility.Caching;
using SOL.Utility.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddCachingSystem(this IServiceCollection services)
    {
        services.AddScoped<ICacheManager, CacheManager>();

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
}

public class StorageSystemOptions
{
    public string? AzureConnectionString { get; set; }
}