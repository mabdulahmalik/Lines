using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.TenantMgmt;

public class EnactTenantCreation : IMessage
{
    public string Name { get; set; }
    public string Key { get; set; }
}