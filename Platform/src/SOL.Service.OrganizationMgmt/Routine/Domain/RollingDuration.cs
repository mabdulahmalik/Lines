using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public class RollingDuration : ValueType<RollingDuration>
{
    public int Value { get; }
    public DurationUnit Unit { get; }

    public RollingDuration(int value, DurationUnit unit)
    {
        Guard.Argument(value).NotNegative();

        Value = value;
        Unit = unit;
    }

    protected override bool EqualsInternal(RollingDuration other)
    {
        return Value == other.Value && Unit == other.Unit;
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Value, Unit);
    }
}