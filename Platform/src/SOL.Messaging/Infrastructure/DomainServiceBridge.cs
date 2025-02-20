using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

public class DomainServiceBridge<TCommand> : CommandHandler<TCommand> 
    where TCommand : class, IMessage
{
    private readonly IScopedMediator _mediator;

    public DomainServiceBridge(ILogger<DomainServiceBridge<TCommand>> logger, IScopedMediator mediator) 
        : base(logger)
    {
        _mediator = mediator;
    }

    protected override async Task HandleAsync(TCommand command, CancellationToken stoppageToken) 
        => await _mediator.Publish(command, stoppageToken);
}