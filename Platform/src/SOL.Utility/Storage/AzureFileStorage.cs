using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using SOL.Abstractions.Application;
using SOL.Abstractions.Storage;

namespace SOL.Utility.Storage;

public class AzureFileStorage : IFileStorage
{
    private readonly string _appRootUrl;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly Lazy<IOperationContext> _operationContext;

    public AzureFileStorage(BlobServiceClient blobServiceClient, IConfiguration configuration, IOperationContextFactory operationContextFactory)
    {
        _blobServiceClient = blobServiceClient;
        _appRootUrl = configuration["AppUrlRoot"]!;
        _operationContext = new(operationContextFactory.Get);
    }
    
    public string GetAbsoluteUrl(string path) => $"{_appRootUrl}/{_operationContext.Value.TenantKey}/{path}";

    public async Task WriteAsync(string path, Stream stream, IDictionary<string, string>? metadata = null, CancellationToken cancellationToken = default)
    {
        var containerName = path.Split('/').First();
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        var filePath = String.Join("/", path.Split('/').Skip(1));
        var blobClient = containerClient.GetBlobClient(filePath);
        
        await blobClient.UploadAsync(stream, 
            httpHeaders: new BlobHttpHeaders {
                ContentType = GetMimeType(filePath)
            },
            metadata: metadata,
            cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var containerName = path.Split('/').First();
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        
        var filePath = String.Join("/", path.Split('/').Skip(1));
        var blobClient = containerClient.GetBlobClient(filePath);
        
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }

    private static string? GetMimeType(string filePath)
    {
        var fileExt = Path.GetExtension(filePath).Trim('.').ToLower();

        switch (fileExt)
        {
            case "png":
                return "image/png";
            
            case "jpg":
            case "jpeg":
                return "image/jpeg";
            
            case "avif":
                return "image/avif";
            
            case "bmp":
                return "image/bmp";
            
            case "gif":
                return "gif";
            
            case "ico":
                return "image/x-icon";
            
            case "svg":
                return "image/svg+xml";
            
            case "tif":
            case "tiff":
                return "image/tiff";
            
            case "webp":
                return "image/webp";
        }

        return null;
    }
}