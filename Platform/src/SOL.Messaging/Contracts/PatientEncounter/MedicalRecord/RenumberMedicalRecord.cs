using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record RenumberMedicalRecord : IMessage
{
    public Guid Id { get; init; }
    public string Number { get; init; }
}