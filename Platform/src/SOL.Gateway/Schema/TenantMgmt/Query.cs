using SOL.Gateway.Schema.TenantMgmt.Objects;
using SOL.Gateway.Schema.TenantMgmt.Resolvers;

namespace SOL.Gateway.Schema.TenantMgmt;

public class QueryTenantsExtensions : ObjectTypeExtension<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("tenant")
            .Description("Gets the Tenant details of the logged in user.")
            .Type<TenantType>()
            .ResolveWith<TenantMgmtResolver>(x => x.Tenant(default!, default!, default!));
    }
}