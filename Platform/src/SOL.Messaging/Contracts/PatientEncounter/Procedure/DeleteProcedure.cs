using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record DeleteProcedure : IMessage
{
    public Guid Id { get; init; }
}