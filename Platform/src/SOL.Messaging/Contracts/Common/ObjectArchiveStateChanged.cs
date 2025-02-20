using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.Common;

public record ObjectArchiveStateChanged : IMessage
{
    public IReadOnlyList<Guid> Ids { get; init; } = new List<Guid>();
    public string Name { get; init; }
    public bool Archived { get; init; }
}