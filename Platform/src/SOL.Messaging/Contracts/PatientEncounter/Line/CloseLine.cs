using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CloseLine : IMessage
{
    public Guid Id { get; init; }
    public Instant RemovedOn { get; init; }
}