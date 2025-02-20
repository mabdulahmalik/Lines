namespace SOL.Abstractions.Domain;

public enum UserAvailability : byte
{
    Offline = 0,
    Free = 1,
    Busy = 2,
    Away = 3
}