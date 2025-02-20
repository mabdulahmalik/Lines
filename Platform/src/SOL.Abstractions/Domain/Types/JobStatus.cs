namespace SOL.Abstractions.Domain;

public enum JobStatus : byte
{
    Canceled = 1,
    Completed = 2,
    Underway = 3,
    Scheduled = 4
}
