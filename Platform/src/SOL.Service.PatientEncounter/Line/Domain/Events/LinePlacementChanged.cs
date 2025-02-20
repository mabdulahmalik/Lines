using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LinePlacementChanged))]
public record LinePlacementChanged : IMessage
{
    internal LinePlacementChanged(Guid id, bool externallyPlaced, LineDwelling dwelling, string? placedBy = null, Instant? placedOn = null)
    {
        LineId = id;
        Dwelling = dwelling;
        ExternallyPlaced = externallyPlaced;
        PlacedBy = placedBy;
        PlacedOn = placedOn;
    }
    
    public Guid LineId { get; }
    public LineDwelling Dwelling { get; }
    public bool ExternallyPlaced { get; }
    public string? PlacedBy { get; }
    public Instant? PlacedOn { get; }
}