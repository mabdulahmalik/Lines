namespace SOL.Abstractions.Domain;

public abstract class ValueType<T> where T : ValueType<T>
{
    protected abstract bool EqualsInternal(T? other);
    protected abstract int GetHashCodeInternal();

    public override bool Equals(object? obj)
    {
        if (!(obj is T)) return false;
        if (ReferenceEquals(this, obj)) return true;

        var valueObject = obj as T;
        return EqualsInternal(valueObject);
    }

    public override int GetHashCode()
    {
        return GetHashCodeInternal();
    }

    public static bool operator ==(ValueType<T> a, ValueType<T> b)
    {
        if (ReferenceEquals(a, null))
            return ReferenceEquals(b, null);

        return a.Equals(b);
    }

    public static bool operator !=(ValueType<T> a, ValueType<T> b)
    {
        return !(a == b);
    }
}