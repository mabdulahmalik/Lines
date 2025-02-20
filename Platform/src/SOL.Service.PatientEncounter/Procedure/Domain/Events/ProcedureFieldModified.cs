using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Procedure.Domain;

public record ProcedureFieldModified : IMessage
{
    internal ProcedureFieldModified(Guid id, IEnumerable<Field> fields)
    {
        ProcedureId = id;
        Fields = fields.ToList();
    }

    public Guid ProcedureId { get; }
    public IReadOnlyList<Field> Fields { get; }
}