using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Procedure.Domain;

public record ProcedureFieldAdded : IMessage
{
    public Guid ProcedureId { get; }
    public IReadOnlyList<Field> Fields { get; }

    internal ProcedureFieldAdded(Guid procedureId, IEnumerable<Field> addedFields)
    {
        ProcedureId = procedureId;
        Fields = addedFields.ToList();
    }
}