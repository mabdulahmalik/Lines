using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure.Schedules;

class SingleScheduleDecorator : ICommandScheduler
{
    private readonly ICommandScheduler _commandScheduler;
    private readonly IEnumerable<ISingleScheduler> _singletons;

    public SingleScheduleDecorator(ICommandScheduler commandScheduler, IEnumerable<ISingleScheduler> singletons)
    {
        _commandScheduler = commandScheduler;
        _singletons = singletons;
    }

    public async Task<Guid> Schedule(object command, ZonedDateTime scheduledDateTime, CancellationToken stoppageToken)
    {
        var singleton = _singletons.FirstOrDefault(s => s.IsSingle(command));
        if (singleton == null)
            return await _commandScheduler.Schedule(command, scheduledDateTime, stoppageToken);
        
        var token = await singleton.IsCached(command);
        if (token.HasValue)
            return token.Value;
        
        var scheduleToken = await _commandScheduler.Schedule(command, scheduledDateTime, stoppageToken);
        await singleton.Cache(command, scheduleToken);

        return scheduleToken;
    }
}