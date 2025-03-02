using Dawn;
using SOL.IdP;
using SOL.IdP.Keycloak;
using SOL.IdP.Phase2;

namespace SOL.Service.TenantMgmt.Tenant.Domain.Services;

public class TenantManager
{
    private readonly IPhase2AdminClient _phase2AdminClient;
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public TenantManager(IPhase2AdminClient phase2AdminClient, IKeycloakAdminClient keycloakAdminClient)
    {
        _phase2AdminClient = phase2AdminClient;
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<Tenant> GetTenantByKey(string tenantKey, CancellationToken stoppageToken = default)
    {
        Guard.Argument(tenantKey).NotNull().NotEmpty().NotWhiteSpace();
        
        var orgs = await _phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId,
            search: tenantKey, cancellationToken: stoppageToken);

        return orgs.Select(Tenant.Map).First();
    }
    
    public async Task EstablishDefaultRoles(string tenantKey, CancellationToken stoppageToken = default)
    {
        Guard.Argument(tenantKey).NotNull().NotEmpty().NotWhiteSpace();
        
        var defaultGroup = await _keycloakAdminClient
            .GroupByPathAsync($"/defaults/Roles", KeycloakConst.RealmId, stoppageToken);

        var defaultGroups = await _keycloakAdminClient
            .ChildrenAllAsync(KeycloakConst.RealmId, defaultGroup.Id, briefRepresentation: true,
                cancellationToken: stoppageToken);

        var tenantGroup = new GroupRepresentation
        {
            Name = tenantKey,
            Path = $"/tenants",
            SubGroups = defaultGroups.Select(dg => new GroupRepresentation
            {
                Name = dg.Name,
                Path = $"/tenants/{tenantKey}/Roles"
            }).ToList()
        };
        
        await _keycloakAdminClient.GroupsPOSTAsync(KeycloakConst.RealmId, tenantGroup, stoppageToken);
    }
}