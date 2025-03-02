using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SOL.Gateway.Views.TenantMgmt;

namespace SOL.Gateway.Schema.TenantMgmt.Objects;

public class TenantType : ObjectType<TenantView>
{
    protected override void Configure(IObjectTypeDescriptor<TenantView> descriptor)
    {
        descriptor
            .Name("Tenant")
            .Description("A customer instance of the application.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Tenant.");
        
        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Tenant.");
        
        descriptor
            .Field(x => x.Key)
            .Name("key")
            .Description("The unique Key of the Tenant.");
        
        descriptor
            .Field(x => x.Active)
            .Name("active")
            .Description("Whether or not the Tenant is Active.");
    }
}