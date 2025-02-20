namespace SOL.Abstractions.Messaging;

public interface ICommandBus
{
    Task SendAsync(object command, CancellationToken stoppageToken);
}