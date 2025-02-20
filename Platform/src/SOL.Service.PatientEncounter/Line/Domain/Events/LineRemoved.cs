using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineRemoved))]
public record LineRemoved : IMessage
{
    public Guid Id { get; }
    public Instant RemovedOn { get; }

    internal LineRemoved(Line line)
    {
        Id = line.Id;
        RemovedOn = line.RemovedOn!.Value;
    }
}