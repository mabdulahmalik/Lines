using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Procedure.Domain;

public record ProcedureFieldRemoved : IMessage
{
    public Guid ProcedureId { get; }
    public IReadOnlyList<Guid> FieldIds { get; }

    internal ProcedureFieldRemoved(Guid procedureId, IEnumerable<Field> addedFields)
    {
        ProcedureId = procedureId;
        FieldIds = addedFields.Select(f => f.Id).ToList();
    }
}