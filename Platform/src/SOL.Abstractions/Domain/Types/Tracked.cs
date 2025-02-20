using NodaTime;

namespace SOL.Abstractions.Domain;

public class Tracked<T> : List<TimeStamped<T>>
{
    public TimeStamped<T> Current => 
        this.OrderByDescending(x => x.TimeStamp).FirstOrDefault()!;
    
    public void Add(T value)
    {
        if (value != null && (Current == null || !value.Equals(Current.Value)))
            Add(new TimeStamped<T>(value));
    }
    
    public void AddRange(IEnumerable<T> values)
    {
        AddRange(values.Select(x => new TimeStamped<T>(x)));
    }
}

public class TimeStamped<TType> : ValueType<TimeStamped<TType>>
{
    public Instant TimeStamp { get; }
    public TType Value { get; }

    internal TimeStamped(TType value, Instant timeStamp)
    {
        TimeStamp = timeStamp;
        Value = value;
    }

    internal TimeStamped(TType value) 
        : this(value, SystemClock.Instance.GetCurrentInstant()) 
    { }

    protected override bool EqualsInternal(TimeStamped<TType> other)
    {
        return TimeStamp.Equals(other.TimeStamp) &&
               Value.Equals(other.Value);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Value, TimeStamp);
    }
}