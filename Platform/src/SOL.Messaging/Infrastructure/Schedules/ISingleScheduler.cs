namespace SOL.Messaging.Infrastructure.Schedules;

public interface ISingleScheduler
{
    bool IsSingle(object command);
    Task<Guid?> IsCached(object command);
    Task Cache(object command, Guid scheduleToken);
}