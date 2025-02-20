namespace SOL.Abstractions.Domain;

[Flags]
public enum ProcedureSetting : byte
{
    PerformanceReporting = 128,
    Insertion = 1,
    Removal = 2
}
