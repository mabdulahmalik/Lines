using MassTransit.Mediator;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

class DomainEventBus : IDomainBus
{
    private readonly IScopedMediator _mediator;

    public DomainEventBus(IScopedMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(object message, CancellationToken stoppageToken) 
        => await _mediator.Publish(message, stoppageToken);
}