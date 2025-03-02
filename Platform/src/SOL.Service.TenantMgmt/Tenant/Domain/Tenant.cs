using SOL.Abstractions.Domain;
using SOL.IdP.Phase2;
using SOL.Service.TenantMgmt.Tenant.Domain.Events;

namespace SOL.Service.TenantMgmt.Tenant.Domain;

public class Tenant : AggregateRoot
{
    private Tenant(Guid id) 
        : base(id)
    { }
    
    public string Name { get; private set; }
    public string Key { get; private set; }
    public bool Active { get; private set; }
    
    public static Tenant Create(string name, string key)
    {
        var tenant = new Tenant(Guid.NewGuid()) {
            Name = name,
            Key = key.ToLower(),
            Active = true
        };
        
        tenant.RaiseEventCreated();
        return tenant;
    }
    
    internal static Tenant Map(OrganizationRepresentation organization)
    {
        var tenant = new Tenant(Guid.Parse(organization.Id)) {
            Name = organization.DisplayName,
            Key = organization.Name.ToLower(),
            Active = true
        };
        
        return tenant;
    }
    
    public void Activate()
    {
        Active = true;
        RaiseEvent(new TenantActivationStatusChanged(Id, true));
    }
    
    public void Deactivate()
    {
        Active = false;
        RaiseEvent(new TenantActivationStatusChanged(Id, true));
    }
}