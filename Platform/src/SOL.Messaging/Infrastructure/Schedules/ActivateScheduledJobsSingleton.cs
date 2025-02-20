using System.Globalization;
using SOL.Abstractions.Application;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Messaging.Infrastructure.Schedules;

public class ActivateScheduledJobsSingleton : ISingleScheduler
{
    private readonly ICacheManager _cacheManager;

    public ActivateScheduledJobsSingleton(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    public bool IsSingle(object command) => command.GetType() == typeof(ActivateScheduledJobs);
    
    public async Task<Guid?> IsCached(object command)
    {
        var activatedSchedule = (ActivateScheduledJobs)command;
        var cacheKey = GetCacheKey(activatedSchedule);
        var token = await _cacheManager.Get(cacheKey);

        return !string.IsNullOrWhiteSpace(token) 
            ? Guid.Parse(token) 
            : null;
    }

    public async Task Cache(object command, Guid scheduleToken)
    {
        var activatedSchedule = (ActivateScheduledJobs)command;
        var cacheKey = GetCacheKey(activatedSchedule);

        await _cacheManager.Set(cacheKey, scheduleToken.ToString()
            , activatedSchedule.Date.ToDateTimeUnspecified());
    }

    private string GetCacheKey(ActivateScheduledJobs command) =>
        $"single-schedule:{nameof(ActivateScheduledJobs)}.{command
            .Date.ToString("yyyyMMdd", new CultureInfo("en-US"))}";
}