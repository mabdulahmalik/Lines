using NodaTime;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record LinePlacementChanged : IMessage
{
    public Guid Id { get; init; }
    public LineDwelling Dwelling { get; init; }
    public bool ExternallyPlaced { get; init; }
    public string? PlacedBy { get; init; }
    public LocalDate? PlacedOn { get; init; }
}