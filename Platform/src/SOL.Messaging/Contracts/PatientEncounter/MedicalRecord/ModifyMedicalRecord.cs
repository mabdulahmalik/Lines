using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ModifyMedicalRecord : IMessage
{
    public Guid? Id { get; init; }
    public string? Number { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public LocalDate? Birthday { get; init; }
}