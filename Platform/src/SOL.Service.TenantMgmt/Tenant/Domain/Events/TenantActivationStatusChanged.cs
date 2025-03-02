using SOL.Abstractions.Messaging;

namespace SOL.Service.TenantMgmt.Tenant.Domain.Events;

public record TenantActivationStatusChanged(Guid Id, bool Active) : IMessage
{
}