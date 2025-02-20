using MassTransit;
using Microsoft.Extensions.Logging;
using NodaTime.Text;
using SOL.Abstractions.Application;
namespace SOL.Messaging.Infrastructure.Filters;

class ConsumeMessageHeadersFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly IOperationContextFactory _operationContextFactory;
    private readonly ILogger<ConsumeMessageHeadersFilter<T>> _logger;

    public ConsumeMessageHeadersFilter(IOperationContextFactory operationContextFactory, ILogger<ConsumeMessageHeadersFilter<T>> logger)
    {
        _operationContextFactory = operationContextFactory;
        _logger = logger;
    }

    public void Probe(ProbeContext context) { }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        _logger.LogInformation("[ConsumeHeaders] ReceiveContext: {ReceiveContext} --- Message: {MessageType} --- CorrelationId: {CorrelationId} --- ConversationId: {ConversationId} -- ThreadId: {ThreadId}"
            , context.ReceiveContext.GetType().Name, context.Message.GetType().FullName, context.CorrelationId, context.ConversationId, Thread.CurrentThread.ManagedThreadId);
        
        if (_operationContextFactory.HasValue) {
            await next.Send(context);
            return;
        }

        if (!context.Headers.Any(hd => hd.Key.StartsWith("SOL-"))) {
            await next.Send(context);
            return;
        }
        
        var instantStr = context.Headers.Get<string>($"SOL-{nameof(IOperationContext.TimeStamp)}");
        var actorStr = context.Headers.Get<string>($"SOL-{nameof(IOperationContext.ActorId)}");

        var opsCtx = new OperationContext { 
            IdentityContext = context.Headers.Get<string>($"SOL-{nameof(IOperationContext.IdentityContext)}"),
            TenantKey = context.Headers.Get<string>($"SOL-{nameof(IOperationContext.TenantKey)}"),
            TraceId = context.Headers.Get<string>($"SOL-{nameof(IOperationContext.TraceId)}"),
            TimeStamp = InstantPattern.General.Parse(instantStr).Value,
            CorrelationId = context.CorrelationId.GetValueOrDefault(),
            ActorId = Guid.Parse(actorStr)
        };

        _operationContextFactory.Set(opsCtx);

        await next.Send(context);
    }
}