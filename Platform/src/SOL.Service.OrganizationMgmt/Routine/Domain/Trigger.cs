using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public class Trigger : ValueType<Trigger>
{
    public Guid ProcedureId { get; }
    public Guid? PropertyId { get; }
    public string? PropertyValue { get; }

    internal Trigger(Guid procedureId, Guid? propertyId, string? propertyValue)
    {
        Guard.Argument(procedureId).NotDefault();

        ProcedureId = procedureId;
        PropertyId = propertyId;
        PropertyValue = propertyValue;
    }

    protected override bool EqualsInternal(Trigger other)
    {
        return ProcedureId == other.ProcedureId && PropertyId == other.PropertyId 
               && String.Equals(PropertyValue, other.PropertyValue, StringComparison.Ordinal);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(ProcedureId, PropertyId, PropertyValue);
    }
}