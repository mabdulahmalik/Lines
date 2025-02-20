using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CreateLine : IMessage
{
    public string? Name { get; init; }
    public string? Type { get; init; }
    public Guid RoomId { get; init; }
    public Instant? InsertedOn { get; init; }
    public Guid? InsertedWith { get; init; }
    public bool ExternallyPlaced { get; init; }
    public string? ExternallyPlacedBy { get; init; }    
    public IEnumerable<Guid> EncounterIds { get; init; } = Array.Empty<Guid>();
}