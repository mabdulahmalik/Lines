using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record UnarchiveProcedure : IMessage
{
    public Guid Id { get; init; }
}