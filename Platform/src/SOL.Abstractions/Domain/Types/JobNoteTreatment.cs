namespace SOL.Abstractions.Domain;

public enum JobNoteTreatment : byte
{
    None = 0,
    Pinned = 1,
    ReasonHold = 2,
    ReasonStat = 3,
    ReasonSos = 4
}