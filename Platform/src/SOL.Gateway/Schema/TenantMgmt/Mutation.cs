using SOL.Gateway.Schema.Common;
using SOL.Gateway.Schema.TenantMgmt.Inputs;
using SOL.Messaging.Contracts.TenantMgmt;

namespace SOL.Gateway.Schema.TenantMgmt;

public class MutationTenantExtensions : ObjectTypeExtension<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field("enactTenantCreation")
            .Description("Provisions a new Tenant and all it's dependent resources.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<EnactTenantCreationPrcType>())
            .ResolveWith<MutationResolver<EnactTenantCreation>>(x => x.Mutate(default!, default!, default!, default!));
    }
}