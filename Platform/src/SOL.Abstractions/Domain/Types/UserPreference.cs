namespace SOL.Abstractions.Domain;

[Flags]
public enum UserPreference : byte
{
    ElapsedTime = 1,
    MilitaryTime = 2,
    MiddleEndianDate = 4
}