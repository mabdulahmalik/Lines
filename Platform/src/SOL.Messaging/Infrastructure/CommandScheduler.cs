using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure;

public class CommandScheduler : ICommandScheduler
{
    private readonly Lazy<IMessageScheduler> _scheduler;
    private readonly Lazy<IOperationContext> _operationContext;
    
    public CommandScheduler(IServiceScopeFactory scope, IOperationContextFactory operationContextFactory)
    {
        _operationContext = new (operationContextFactory.Get);
        _scheduler = new Lazy<IMessageScheduler>(() => 
            scope.CreateScope().ServiceProvider
                .GetRequiredService<IMessageScheduler>()
        );
    }
    
    public async Task<Guid> Schedule(object command, ZonedDateTime scheduledDateTime, CancellationToken stoppageToken)
    {
        var publishPipe = Pipe.Execute<SendContext>(ctx => {
            ctx.Headers.Set("SOL-IdentityContext", _operationContext.Value.IdentityContext);
            ctx.Headers.Set("SOL-TenantKey", _operationContext.Value.TenantKey);
            ctx.Headers.Set("SOL-TraceId", _operationContext.Value.TraceId);
            ctx.Headers.Set("SOL-ActorId", _operationContext.Value.ActorId.ToString());
            ctx.Headers.Set("SOL-TimeStamp", _operationContext.Value.TimeStamp.ToString());
            ctx.Headers.Set("SOL-CorrelationId", _operationContext.Value.CorrelationId.ToString());
        });
        
        var scheduledTime = scheduledDateTime.ToDateTimeUtc();
        var scheduledMessage = await _scheduler.Value.SchedulePublish(scheduledTime
            , command, publishPipe, stoppageToken);

        return scheduledMessage.TokenId;
    }
}