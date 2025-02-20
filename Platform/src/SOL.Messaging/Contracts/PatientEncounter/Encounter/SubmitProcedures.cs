using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record SubmitProcedures : IMessage
{
    public Guid EncounterId { get; init; }
}