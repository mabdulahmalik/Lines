using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record ModifyProcedure : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool EnablePerformanceReporting { get; init; }
    public List<RequiredPatientData> RequiredData { get; init; } = new();
    public List<ModifyProcedureField> Fields { get; init; } = new();
}

public record ModifyProcedureField
{
    public Guid? Id { get; init; }
    public string Name { get; init; }
    public string? Instruction { get; init; }
    public ProcedureFieldType Type { get; init; }
    public bool Archived { get; init; }
    public bool Required { get; init; }
    public bool AllowMultiSelect { get; init; }
    public List<ModifyProcedureFieldOption>? Options { get; init; }
}

public class ModifyProcedureFieldOption
{
    public Guid? Id { get; init; }
    public string Text { get; init; }
    public bool Archived { get; init; }
}