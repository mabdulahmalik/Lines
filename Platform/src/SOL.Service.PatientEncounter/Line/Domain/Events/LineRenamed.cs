using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineRenamed))]
public record LineRenamed : IMessage
{
    public Guid Id { get; }
    public string Name { get; }

    internal LineRenamed(Line line)
    {
        Id = line.Id;
        Name = line.Name;
    }
}