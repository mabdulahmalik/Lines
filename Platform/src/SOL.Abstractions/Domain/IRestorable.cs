using NodaTime;

namespace SOL.Abstractions.Domain;

/// <summary>
/// Used as a means to identify Domain Aggregates that can be Restored (from being deleted).
/// </summary>
public interface IRestorable
{
    Instant? DeletedAt { get; }
}