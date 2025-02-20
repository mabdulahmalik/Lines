using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RemoveProcedureFromEncounter : IMessage
{
    public Guid Id { get; init; }
    public Guid EncounterId { get; init; }
}
