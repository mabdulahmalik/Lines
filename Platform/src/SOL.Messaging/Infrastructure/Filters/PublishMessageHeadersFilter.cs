using MassTransit;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;

namespace SOL.Messaging.Infrastructure.Filters;

class PublishMessageHeadersFilter<T> : IFilter<PublishContext<T>>
    where T : class
{
    private readonly IOperationContextFactory _operationContextFactory;
    private readonly ILogger<PublishMessageHeadersFilter<T>> _logger;

    public PublishMessageHeadersFilter(IOperationContextFactory operationContextFactoryFactory, ILogger<PublishMessageHeadersFilter<T>> logger)
    {
        _operationContextFactory = operationContextFactoryFactory;
        _logger = logger;
    }

    public void Probe(ProbeContext context) { }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        _logger.LogInformation("[PublishHeaders] Message: {MessageType} --- CorrelationId: {CorrelationId} --- ConversationId: {ConversationId} -- ThreadId: {ThreadId}"
            , context.Message.GetType().FullName, context.CorrelationId, context.ConversationId, Thread.CurrentThread.ManagedThreadId);
        
        if (context.Headers.Any(hd => hd.Key.StartsWith("SOL-"))) {
            if (!context.CorrelationId.HasValue) {
                var corrIdStr = context.Headers.Get<string>($"SOL-{nameof(IOperationContext.CorrelationId)}");
                context.CorrelationId = Guid.Parse(corrIdStr!);
            }
            await next.Send(context);
            return;
        }
        
        if (!_operationContextFactory.HasValue) {
            await next.Send(context);
            return;
        }        
        
        var opsCtx = _operationContextFactory.Get();

        context.Headers.Set($"SOL-{nameof(IOperationContext.IdentityContext)}", opsCtx.IdentityContext);
        context.Headers.Set($"SOL-{nameof(IOperationContext.TimeStamp)}", opsCtx.TimeStamp.ToString());
        context.Headers.Set($"SOL-{nameof(IOperationContext.CorrelationId)}", opsCtx.CorrelationId);
        context.Headers.Set($"SOL-{nameof(IOperationContext.TenantKey)}", opsCtx.TenantKey);
        context.Headers.Set($"SOL-{nameof(IOperationContext.ActorId)}", opsCtx.ActorId);
        context.Headers.Set($"SOL-{nameof(IOperationContext.TraceId)}", opsCtx.TraceId);
        context.CorrelationId = opsCtx.CorrelationId;

        await next.Send(context);
    }
}