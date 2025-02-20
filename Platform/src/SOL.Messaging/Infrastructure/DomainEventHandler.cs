using MassTransit;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

public abstract class DomainEventHandler<TDomainEvent> : IConsumer<TDomainEvent>, IDomainHandler
    where TDomainEvent : class, IMessage
{
    protected readonly ILogger Log;

    protected DomainEventHandler(ILogger logger)
    {
        Log = logger;
    }

    public async Task Consume(ConsumeContext<TDomainEvent> context)
    {
        await HandleAsync(context.Message, context.CancellationToken);
    }

    protected abstract Task HandleAsync(TDomainEvent message, CancellationToken stoppageToken);
}