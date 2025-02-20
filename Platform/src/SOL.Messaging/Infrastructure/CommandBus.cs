using MassTransit;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

class CommandBus : ICommandBus
{
    private readonly IBus _bus;

    public CommandBus(IBus bus)
    {
        _bus = bus;
    }    

    public async Task SendAsync(object command, CancellationToken stoppageToken)
    {
        await _bus.Publish(command, stoppageToken);
    }
}