using NodaTime;

namespace SOL.Abstractions.Application;

public class OperationContext : IOperationContext
{
    public string IdentityContext { get; init; }
    public string TraceId  { get; init; }
    public Instant TimeStamp { get; init; }
    public Guid CorrelationId { get; init; }
    public Guid ActorId { get; init; }
    public string TenantKey { get; init; }
}