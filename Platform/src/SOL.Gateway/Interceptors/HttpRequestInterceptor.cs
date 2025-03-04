using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using NodaTime;
using SOL.Abstractions.Application;
using System.Diagnostics;
using HotChocolate.Resolvers;

namespace SOL.Gateway.Interceptors;

class HttpRequestInterceptor : DefaultHttpRequestInterceptor
{
    private readonly IClock _clock;
    private readonly IOperationContextFactory _operationContextFactory;

    public HttpRequestInterceptor(IClock clock, IOperationContextFactory operationContextFactory)
    {
        _clock = clock;
        _operationContextFactory = operationContextFactory;
    }

    public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor
        , OperationRequestBuilder requestBuilder, CancellationToken cancellationToken)
    {
        _operationContextFactory.Set(new OperationContext {
            IdentityContext = "LiNES",
            TraceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N"),
            TimeStamp = _clock.GetCurrentInstant(),
            CorrelationId = Guid.NewGuid(),
            TenantKey = "demo",
            ActorId = Guid.Parse("30400edb-a083-4efc-a823-86b6599e8811")            
        });   
        
        return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}