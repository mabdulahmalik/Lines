using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public class Photo : Entity
{
    public Instant CreatedAt { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public long Length { get; private set; }

    internal Photo(Guid id, Instant createdAt, string fileName, string contentType, long length) 
        : base(id)
    {
        CreatedAt = createdAt;
        FileName = fileName;
        ContentType = contentType;
        Length = length;
    }

    internal Photo(string fileName, string contentType, long length) 
        : this(Guid.NewGuid(), SystemClock.Instance.GetCurrentInstant(), fileName, contentType, length) 
    { }
}