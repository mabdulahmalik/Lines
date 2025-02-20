using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record LineRemoved : IMessage
{
    public Guid Id { get; init; }
    public LocalDate RemovedOn { get; init; }
}