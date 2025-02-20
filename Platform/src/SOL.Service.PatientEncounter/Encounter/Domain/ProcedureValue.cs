using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public class ProcedureValue : ValueType<ProcedureValue>
{
    public Guid FieldId { get; }
    public string Value { get; private set; }

    internal ProcedureValue(Guid fieldId, string value)
    {
        FieldId = fieldId;
        Value = value;
    }

    internal void SetValue(string value)
    {
        Value = value;
    }

    protected override bool EqualsInternal(ProcedureValue other)
    {
        return FieldId.Equals(other.FieldId)
            && Value.Equals(other.Value, StringComparison.CurrentCultureIgnoreCase);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(FieldId, Value);
    }
}
