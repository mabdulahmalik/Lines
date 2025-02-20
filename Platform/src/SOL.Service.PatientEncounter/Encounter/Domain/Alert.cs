using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public class Alert : ValueType<Alert>
{
    public EncounterAlertType Type { get; }
    public Instant AlertedAt { get; }
    public Guid AlertedBy { get; }
    public string? Text { get; }
    
    internal Alert(EncounterAlertType type, Instant alertedAt, Guid alertedBy, string? text = null)
    {
        Type = type;
        Text = text;
        AlertedBy = alertedBy;
        AlertedAt = alertedAt;
    }
    
    internal Alert(EncounterAlertType type, Guid alertedBy, string? text = null)
        : this(type, SystemClock.Instance.GetCurrentInstant(), alertedBy, text)
    { }

    protected override bool EqualsInternal(Alert? other)
    {
        return Type.Equals(other?.Type)
               && AlertedBy.Equals(other?.AlertedBy)
               && AlertedAt.Equals(other?.AlertedAt)
               && String.Compare(Text, other?.Text, StringComparison.CurrentCultureIgnoreCase) == 0;
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Type, Text, AlertedAt, AlertedBy);
    }
}