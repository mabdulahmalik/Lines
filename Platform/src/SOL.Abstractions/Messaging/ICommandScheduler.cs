using NodaTime;

namespace SOL.Abstractions.Messaging;

public interface ICommandScheduler
{
    Task<Guid> Schedule(object command, ZonedDateTime scheduledDateTime, CancellationToken stoppageToken);
}