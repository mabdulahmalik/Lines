using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterPhotosAttached : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<EncounterPhoto> Photos { get; init; } = new List<EncounterPhoto>();
}

public record EncounterPhoto
{
    public Guid Id { get; init; }
    public string Url { get; init; }
    public Instant CreatedAt { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public long Length { get; init; }    
}