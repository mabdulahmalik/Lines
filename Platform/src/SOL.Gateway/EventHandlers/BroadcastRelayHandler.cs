using SOL.Abstractions.Messaging;
using SOL.Gateway.Application;
using SOL.Messaging.Infrastructure;

namespace SOL.Gateway.EventHandlers;

public class BroadcastRelayHandler<TEvent> : ServiceEventHandler<TEvent>
    where TEvent : class, IMessage
{
    private readonly ISubscriptionHub _subscriptionHub;

    public BroadcastRelayHandler(ILogger<BroadcastRelayHandler<TEvent>> logger
        , ISubscriptionHub subscriptionHub) 
        : base(logger)
    {
        _subscriptionHub = subscriptionHub;
    }

    protected override async Task HandleAsync(TEvent message, CancellationToken stoppageToken) 
        => await _subscriptionHub.SendBroadcast(message, stoppageToken);
}