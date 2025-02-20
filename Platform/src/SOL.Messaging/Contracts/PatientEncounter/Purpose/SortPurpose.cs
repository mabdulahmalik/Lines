using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record SortPurpose : IMessage
{
    public Guid Id { get; init; }
    public int From { get; init; }
    public int To { get; init; }    
}