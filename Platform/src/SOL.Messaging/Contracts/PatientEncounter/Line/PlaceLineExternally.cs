using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record PlaceLineExternally : IMessage
{
    public Guid Id { get; init; }
    public string? PlacedBy { get; init; }
    public Instant PlacedOn { get; init; }
}