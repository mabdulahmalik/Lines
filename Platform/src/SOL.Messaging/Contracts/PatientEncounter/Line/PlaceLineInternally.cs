using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record PlaceLineInternally : IMessage
{
    public Guid Id { get; init; }
}