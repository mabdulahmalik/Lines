using MassTransit;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

class ServiceEventBus : IServiceBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ServiceEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(object message, CancellationToken stoppageToken)
    {
        await _publishEndpoint.Publish(message, stoppageToken);
    }
}