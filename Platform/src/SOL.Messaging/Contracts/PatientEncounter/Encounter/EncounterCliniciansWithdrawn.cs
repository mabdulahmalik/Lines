using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record EncounterCliniciansWithdrawn : IMessage
{
    public Guid Id { get; init; }
    public IReadOnlyList<Guid> ClinicianIds { get; init; }
}