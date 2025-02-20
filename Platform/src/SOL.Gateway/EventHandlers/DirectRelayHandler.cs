using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;
using SOL.Gateway.Application;
using SOL.Messaging.Infrastructure;

namespace SOL.Gateway.EventHandlers;

public class DirectRelayHandler<TEvent> : ServiceEventHandler<TEvent>
    where TEvent : class, IMessage
{
    private readonly ISubscriptionHub _subscriptionHub;
    private readonly Lazy<IOperationContext> _operationContext;
    
    public DirectRelayHandler(ILogger<DirectRelayHandler<TEvent>> logger
        , ISubscriptionHub subscriptionHub
        , IOperationContextFactory operationContextFactory) 
        : base(logger)
    {
        _subscriptionHub = subscriptionHub;
        _operationContext = new(operationContextFactory.Get);
    }
    
    protected override async Task HandleAsync(TEvent message, CancellationToken stoppageToken) 
        => await _subscriptionHub.SendDirect(message, _operationContext.Value.ActorId, stoppageToken);
}