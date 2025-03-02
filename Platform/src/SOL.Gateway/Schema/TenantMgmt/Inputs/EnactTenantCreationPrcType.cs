using SOL.Messaging.Contracts.TenantMgmt;

namespace SOL.Gateway.Schema.TenantMgmt.Inputs;

public class EnactTenantCreationPrcType : InputObjectType<EnactTenantCreation>
{
    protected override void Configure(IInputObjectTypeDescriptor<EnactTenantCreation> descriptor)
    {
        descriptor
            .Name("EnactTenantCreationPrc")
            .Description("Command to create a Tenant.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("Name to give the new Tenant.");
        
        descriptor
            .Field(x => x.Key)
            .Name("key")
            .Description("Key for the Tenant (must be unique).");
    }
}