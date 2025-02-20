using System.ComponentModel;
using Dawn;
using Microsoft.EntityFrameworkCore.SqlServer.NodaTime.Extensions;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public class RollingSchedule : ValueType<RollingSchedule>
{
    public RollingDuration Interval { get; init; }
    public RollingDuration Delay { get; init; }

    public RollingSchedule(RollingDuration interval, RollingDuration delay)
    {
        Guard.Argument(interval).NotNull();
        Guard.Argument(delay).NotNull();

        Interval = interval;
        Delay = delay;
    }

    // This private constructor is required for EF Core, do not remove
    private RollingSchedule()
    {
        Interval = default!;
        Delay = default!;
    }

    public Instant NextRun(Instant seed = default)
    {
        if (seed == default)
            seed = SystemClock.Instance.GetCurrentInstant();
        
        return Interval.Unit switch {
            DurationUnit.Days => seed.PlusDays(Interval.Value),
            DurationUnit.Hours => seed.PlusHours(Interval.Value),
            DurationUnit.Minutes => seed.PlusMinutes(Interval.Value),
            _ => throw new InvalidEnumArgumentException()
        };
    }
    
    public Instant StartDelay(Instant seed = default)
    {
        if (seed == default)
            seed = SystemClock.Instance.GetCurrentInstant();
        
        return Delay.Unit switch {
            DurationUnit.Days => seed.PlusDays(Delay.Value),
            DurationUnit.Hours => seed.PlusHours(Delay.Value),
            DurationUnit.Minutes => seed.PlusMinutes(Delay.Value),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    protected override bool EqualsInternal(RollingSchedule other)
    {
        return Interval.Equals(other.Interval) && Delay.Equals(other.Delay);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Interval, Delay);
    }
}