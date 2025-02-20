namespace SOL.Abstractions.Messaging;

public interface IDomainBus
{
    Task DispatchAsync(object message, CancellationToken stoppageToken);
}