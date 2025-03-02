namespace Microsoft.Extensions.DependencyInjection;

public record ConnectionStrings
{
    public string? TenantDb { get; set; }
    public string? RedisCache { get; set; }
    public string? AzureStorage { get; set; }
    public string? AzureServiceBus { get; set; }
    public string? AppInsights { get; set; }
}