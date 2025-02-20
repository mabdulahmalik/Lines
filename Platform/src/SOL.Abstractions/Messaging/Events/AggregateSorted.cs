using SOL.Abstractions.Messaging;

namespace SOL.Abstractions.Domain;

public record AggregateSorted : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int PosOld { get; init; }
    public int PosNew { get; init; }
}