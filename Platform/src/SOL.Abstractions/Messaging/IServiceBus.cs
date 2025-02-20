namespace SOL.Abstractions.Messaging;

public interface IServiceBus
{
    Task PublishAsync(object message, CancellationToken stoppageToken);
}