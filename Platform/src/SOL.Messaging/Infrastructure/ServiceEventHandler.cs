using MassTransit;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

public abstract class ServiceEventHandler<TServiceEvent> : IConsumer<TServiceEvent>, IServiceHandler
    where TServiceEvent : class
{
    protected readonly ILogger Log;

    protected ServiceEventHandler(ILogger logger)
    {
        Log = logger;
    }

    public async Task Consume(ConsumeContext<TServiceEvent> context)
    {
        await HandleAsync(context.Message, context.CancellationToken);
    }

    protected abstract Task HandleAsync(TServiceEvent message, CancellationToken stoppageToken);
}