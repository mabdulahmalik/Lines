using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record CreateProcedure : IMessage
{
    public string Name { get; init; }
    public bool EnablePerformanceReporting { get; init; }
    public bool IsInsertion { get; init; }
    public bool IsRemoval { get; init; }
    public List<RequiredPatientData> RequiredData { get; init; } = new();
    public List<ProcedureField> Fields { get; init; } = new();
}

public record ProcedureField
{
    public string Name { get; init; }
    public string? Instruction { get; init; }
    public ProcedureFieldType Type { get; init; }
    public bool Required { get; init; }
    public bool AllowMultiSelect { get; init; }
    public List<string>? Options { get; init; }
}