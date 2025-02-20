using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public class Procedure : Entity
{
    private readonly List<ProcedureValue> _values = new();

    public Instant PerformedAt { get; }
    public Guid PerformedBy { get; }
    public Guid ProcedureId { get; }
    public IReadOnlyList<ProcedureValue> Values => _values;

    internal Procedure(Guid id, Guid procedureId, Guid performedBy, Instant performedAt, params ProcedureValue[] values)
        : base(id)
    {
        ProcedureId = procedureId;
        PerformedBy = performedBy;
        PerformedAt = performedAt;
        _values = values.ToList();
    }

    internal Procedure(Guid procedureId, Guid performedBy, params ProcedureValue[] values)
        : this(Guid.NewGuid(), procedureId, performedBy, SystemClock.Instance.GetCurrentInstant(), values)
    { }

    internal Procedure(Guid procedureId, Guid performedBy)
        : this(Guid.NewGuid(), procedureId, performedBy, SystemClock.Instance.GetCurrentInstant())
    { }
    
    public void SetValues(params ProcedureValue[] values)
    {
        _values.Clear();
        _values.AddRange(values);
    }
}