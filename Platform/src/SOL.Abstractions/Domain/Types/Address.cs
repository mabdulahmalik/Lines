namespace SOL.Abstractions.Domain;

public class Address : ValueType<Address>
{
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }

    protected override bool EqualsInternal(Address other)
    {
        return Street.Equals(other.Street, StringComparison.CurrentCultureIgnoreCase)
            && City.Equals(other.City, StringComparison.CurrentCultureIgnoreCase)
            && State.Equals(other.State, StringComparison.CurrentCultureIgnoreCase)
            && ZipCode.Equals(other.ZipCode, StringComparison.CurrentCultureIgnoreCase);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Street, City, State, ZipCode);
    }
}