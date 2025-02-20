using SOL.Service.OrganizationMgmt;
using SOL.Utility.Geography;

namespace SOL.Gateway.Schema.Common;

public class USStateType : ObjectType<USState>
{
    protected override void Configure(IObjectTypeDescriptor<USState> descriptor)
    {
        descriptor
            .Name("State")
            .Description("A US State.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the US State.");

        descriptor
            .Field(x => x.Code)
            .Name("code")
            .Description("The 02 letter code of the US State.");
    }
}

