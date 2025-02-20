using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Facility.Domain;

public class AssignmentSpec : ValueType<AssignmentSpec>
{
    public Guid PropertyId { get; }
    public Guid PropertyValue { get; private set; }

    internal AssignmentSpec(Guid propertyId, Guid propertyValue)
    {
        Guard.Argument(propertyId).NotDefault();
        Guard.Argument(propertyValue).NotDefault();

        PropertyId = propertyId;
        PropertyValue = propertyValue;
    }

    protected override bool EqualsInternal(AssignmentSpec other)
    {
        return PropertyId.Equals(other.PropertyId)
            && PropertyValue.Equals(other.PropertyValue);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(PropertyId, PropertyValue);
    }

    internal void SetPropertyValue(Guid value)
    {
        PropertyValue = value;
    }
}
