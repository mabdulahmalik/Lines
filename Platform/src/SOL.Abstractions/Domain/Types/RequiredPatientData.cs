namespace SOL.Abstractions.Domain;

[Flags]
public enum RequiredPatientData : byte
{
    MRN = 1,
    FirstName = 2,
    LastName = 4,
    DateOfBirth = 8,
    OrderingProvider = 16
}
