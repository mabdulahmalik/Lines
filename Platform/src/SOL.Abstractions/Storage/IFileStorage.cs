namespace SOL.Abstractions.Storage;

public interface IFileStorage
{
    Task Create(string partitionName);
    
    string GetAbsoluteUrl(string path);
    
    Task WriteAsync(string path, Stream stream, IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
}