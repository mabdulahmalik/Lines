using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record JobStatusChanged : IMessage
{
    public Guid Id { get; init; }
    public JobStatus Status { get; init; }
}