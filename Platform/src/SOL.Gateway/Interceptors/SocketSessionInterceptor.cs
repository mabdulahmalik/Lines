using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using NodaTime;
using SOL.Abstractions.Application;
using System.Diagnostics;
using HotChocolate.AspNetCore.Subscriptions;

namespace SOL.Gateway.Interceptors;

class SocketSessionInterceptor : DefaultSocketSessionInterceptor
{
    private readonly IClock _clock;
    private readonly IOperationContextFactory _operationContextFactory;

    public SocketSessionInterceptor(IClock clock, IOperationContextFactory operationContextFactory)
    {
        _clock = clock;
        _operationContextFactory = operationContextFactory;
    }

    public override ValueTask OnRequestAsync(ISocketSession session, string operationSessionId
        , OperationRequestBuilder requestBuilder, CancellationToken cancellationToken = new CancellationToken())
    {
        _operationContextFactory.Set(new OperationContext {
            IdentityContext = "LiNES",
            TraceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N"),
            TimeStamp = _clock.GetCurrentInstant(),
            CorrelationId = Guid.NewGuid(),
            TenantKey = "vasaccsol",
            ActorId = Guid.Parse("462b8b01-ecd7-4140-9e03-420a672ef35a")            
        });   
        
        return base.OnRequestAsync(session, operationSessionId, requestBuilder, cancellationToken);
    }
}