namespace SOL.Abstractions.Domain;

public enum EncounterStage : byte
{
    Waiting = 1,
    Assigned = 2,
    Attending = 3,
    Charting = 4,
    Completed = 5
}