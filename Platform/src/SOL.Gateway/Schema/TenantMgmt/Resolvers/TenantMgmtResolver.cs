using SOL.Abstractions.Application;
using SOL.Gateway.Views.TenantMgmt;
using SOL.IdP;
using SOL.IdP.Phase2;

namespace SOL.Gateway.Schema.TenantMgmt.Resolvers;

public class TenantMgmtResolver
{
    public async Task<TenantView> Tenant(IOperationContextFactory operationContextFactory
        , IPhase2AdminClient phase2AdminClient, CancellationToken cancellationToken)
    {
        var optCtx = operationContextFactory.Get();
        var orgs = await phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId, search: optCtx.TenantKey,
            cancellationToken: cancellationToken);
        var org = orgs.First();
        
        return new TenantView
        {
            Id = Guid.Parse(org.Id),
            Name = org.DisplayName,
            Key = org.Name,
            Active = true
        };
    }
}