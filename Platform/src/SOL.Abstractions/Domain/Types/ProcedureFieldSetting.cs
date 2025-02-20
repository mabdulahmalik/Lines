namespace SOL.Abstractions.Domain;

[Flags]
public enum ProcedureFieldSetting : byte
{
    Required = 1,
    MultiSelect = 2
}