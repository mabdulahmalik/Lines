using NodaTime;

namespace SOL.Abstractions.Application;

public interface IOperationContext
{
        string IdentityContext { get; }
        string TraceId { get; }
        Instant TimeStamp { get; }
        Guid CorrelationId { get; }
        Guid ActorId { get; }
        string TenantKey { get; }
}