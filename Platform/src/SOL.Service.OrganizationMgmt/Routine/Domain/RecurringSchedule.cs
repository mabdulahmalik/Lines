using Dawn;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public class RecurringSchedule : List<Recurrence>
{
    internal RecurringSchedule() { }

    public RecurringSchedule(Recurrence[] recurrences)
    {
        Guard.Argument(recurrences).NotNull().NotEmpty();
        AddRange(recurrences);
    }
    
    public ZonedDateTime NextRun(DateTimeZone timeZone, Instant seed = default)
    {
        if (seed == default)
            seed = SystemClock.Instance.GetCurrentInstant();

        return this.Min(x => x.NextRun(timeZone, seed));
    }
}

public class Recurrence : ValueType<Recurrence>
{
    public IsoDayOfWeek[] Days { get; }
    public LocalTime Time { get; }

    public Recurrence(LocalTime time, params IsoDayOfWeek[] days)
    {
        Guard.Argument(time).NotDefault();
        Guard.Argument(days).NotNull().NotEmpty();

        Days = days;
        Time = time;
    }
    
    public ZonedDateTime NextRun(DateTimeZone timeZone, Instant seed = default)
    {
        if (seed == default)
            seed = SystemClock.Instance.GetCurrentInstant();

        var zonedSeed = seed.InZone(timeZone);
        var next = zonedSeed.Date.At(Time);

        if (next > zonedSeed.LocalDateTime)
            return next.InZoneLeniently(timeZone);
        
        while (!Days.Contains(next.DayOfWeek)) {
            next = next.PlusDays(1);
        }

        return next.InZoneLeniently(timeZone);
    }

    protected override bool EqualsInternal(Recurrence other)
    {
        return Time.Equals(other.Time)
            && Days.Except(other.Days).Any();
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Days, Time);
    }
}